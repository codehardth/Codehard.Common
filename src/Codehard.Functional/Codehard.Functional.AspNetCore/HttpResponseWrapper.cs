using System.Text;
using Microsoft.AspNetCore.Http;

namespace Codehard.Functional.AspNetCore;

public interface IHttpResponseWrapper
{
    Task WriteAsync(string text, Encoding encoding, CancellationToken cancellationToken);
    IHeaderDictionary Headers { get; }
    int StatusCode { get; set; }
    
    string ContentType { get; set; }
}

public class HttpResponseWrapper : IHttpResponseWrapper
{
    private readonly HttpResponse response;

    public HttpResponseWrapper(HttpResponse response)
    {
        this.response = response;
    }

    public Task WriteAsync(string text, Encoding encoding, CancellationToken cancellationToken)
    {
        return response.WriteAsync(text, encoding, cancellationToken);
    }

    public IHeaderDictionary Headers => response.Headers;

    public int StatusCode
    {
        get => response.StatusCode;
        set => response.StatusCode = value;
    }
    
    public string ContentType
    {
        get => response.ContentType;
        set => response.ContentType = value;
    }
}