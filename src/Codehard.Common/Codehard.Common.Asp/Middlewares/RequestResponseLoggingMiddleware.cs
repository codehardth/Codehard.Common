using System.Buffers;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Codehard.Common.Asp.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestResponseLoggingMiddleware> logger;

    private static readonly string[] RedactedKeywords =
    {
        "password",
        "devicecode",
        "accesstoken",
        "refreshtoken",
    };

    public RequestResponseLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestResponseLoggingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var request = await FormatRequest(context.Request);

        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();

        context.Response.Body = responseBody;

        await this.next(context);

        var (statusCode, message) = await FormatResponse(context.Response);

        var logLevel = statusCode switch
        {
            >= 400 and <= 499 => LogLevel.Warning,
            >= 500 and <= 599 => LogLevel.Error,
            _ => LogLevel.Debug,
        };

        this.logger.Log(logLevel, request);
        this.logger.Log(logLevel, message);

        await responseBody.CopyToAsync(originalBodyStream);
    }

    private static async Task<string> FormatRequest(HttpRequest request)
    {
        const int maximumReadContentCount = 2048;

        request.EnableBuffering();

        try
        {
            var bodyAsText = string.Empty;

            switch (request.ContentType)
            {
                case var ct when string.IsNullOrWhiteSpace(ct) || ct.StartsWith("multipart/form-data"):
                {
                    break;
                }

                default:
                {
                    var contentLength = Math.Clamp((int)(request.ContentLength ?? 0), 0, maximumReadContentCount);

                    var buffer = ArrayPool<byte>.Shared.Rent(contentLength);

                    try
                    {
                        _ = await request.Body.ReadAsync(buffer, 0, contentLength);

                        // ArrayPool may return larger array size than requested, so we need to slice it to exactly what we asked for
                        // to prevent over-read bytes.
                        bodyAsText = Encoding.UTF8.GetString(buffer[..contentLength]);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buffer, true);
                    }

                    break;
                }
            }

            var redactedQueryString = RedactSensitiveData(request.QueryString.Value);
            var redactedBodyString = RedactSensitiveData(bodyAsText);

            return $"{request.Scheme} {request.Host}{request.Path} {redactedQueryString} {redactedBodyString}";
        }
        catch (Exception ex)
        {
            return $"Unable to extract request body due to {ex.Message}";
        }
        finally
        {
            request.Body.Position = 0;
        }
    }

    private static async Task<(int StatusCode, string Message)> FormatResponse(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);

        if (response.StatusCode is < 400 or > 599)
        {
            return (response.StatusCode, string.Empty);
        }

        using var reader = new StreamReader(response.Body, leaveOpen: true);
        var text = await reader.ReadToEndAsync();

        response.Body.Seek(0, SeekOrigin.Begin);

        return (response.StatusCode, $"{response.StatusCode}: {text}");
    }

    private static string RedactSensitiveData(string? src)
    {
        if (string.IsNullOrWhiteSpace(src))
        {
            return string.Empty;
        }

        var redactRegex = $@"((?i:{string.Join('|', RedactedKeywords)})=)(.*?)(\&|$)";

        return Regex.Replace(src, redactRegex, "$1[REDACTED]$3", RegexOptions.Compiled);
    }
}