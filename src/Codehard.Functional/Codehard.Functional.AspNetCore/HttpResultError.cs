using System.Runtime.Serialization;

namespace Codehard.Functional.AspNetCore;

/// <summary>
/// An HTTP error object.  
/// </summary>
[Serializable]
public record HttpResultError : Error, ISerializable
{
    /// <summary>
    /// A result of HTTP status code for this error.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// An optional error code.
    /// </summary>
    public Option<string> ErrorCode { get; }

    /// <summary>
    /// An optional error data.
    /// </summary>
    public Option<object> Data { get; }

    /// <inheritdoc/>
    public override int Code { get; }

    /// <inheritdoc/>
    public override string Message { get; }

    /// <inheritdoc/>
    public override Option<Error> Inner { get; }

    /// <inheritdoc/>
    public override bool IsExceptional =>
        (int)this.StatusCode >= 500;

    /// <inheritdoc/>
    public override bool IsExpected => !this.IsExceptional;

    private HttpResultError(
        HttpStatusCode statusCode,
        string message,
        Option<string> errorCode,
        Option<object> data,
        Option<Error> inner)
    {
        this.Code = (int)statusCode;
        this.StatusCode = statusCode;
        this.Message = message;
        this.ErrorCode = errorCode;
        this.Data = data;
        this.Inner = inner;
    }

    /// <inheritdoc/>
    public HttpResultError(SerializationInfo info, StreamingContext context)
    {
        this.Code = info.GetInt32(nameof(this.Code));
        this.Message = info.GetString(nameof(this.Message)) ?? string.Empty;
        this.StatusCode =
            (HttpStatusCode)(info.GetValue(nameof(this.StatusCode), typeof(HttpStatusCode))
                             ?? throw new InvalidOperationException());
        this.ErrorCode = info.GetString(nameof(this.ErrorCode));
        this.Inner =
            (Option<Error>)(info.GetValue(nameof(this.Inner), typeof(Option<Error>))
                            ?? throw new InvalidOperationException());
    }

    /// <inheritdoc/>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(this.Code), this.Code);
        info.AddValue(nameof(this.Message), this.Message);
        info.AddValue(nameof(this.StatusCode), this.StatusCode);
        info.AddValue(nameof(this.ErrorCode), this.ErrorCode);
        info.AddValue(nameof(this.Inner), this.Inner);
    }

    /// <inheritdoc/>
    public override ErrorException ToErrorException() =>
        IsExceptional
            ? new WrappedErrorExceptionalException(this)
            : new WrappedErrorExpectedException(this);

    /// <summary>
    /// Creates a new instance of <see cref="HttpResultError"/>.
    /// </summary>
    /// <param name="statusCode">The HTTP status code associated with the error.</param>
    /// <param name="message">The error message.</param>
    /// <param name="errorCode">An optional error code.</param>
    /// <param name="data">Optional additional data related to the error.</param>
    /// <param name="inner">An optional inner error.</param>
    /// <returns>A new instance of <see cref="HttpResultError"/>.</returns>
    public static HttpResultError New(
        HttpStatusCode statusCode,
        string message,
        Option<string> errorCode = default,
        Option<object> data = default,
        Option<Error> inner = default)
    {
        return
            new HttpResultError(
                statusCode,
                message,
                errorCode,
                data,
                inner);
    }
}
