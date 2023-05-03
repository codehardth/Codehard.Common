using System.Runtime.Serialization;

namespace Codehard.Functional.AspNetCore;

[Serializable]
public record HttpResultError : Error, ISerializable
{
    public HttpStatusCode StatusCode { get; }
    
    public Option<string> ErrorCode { get; }
    
    public Option<object> Data { get; }

    public override int Code { get; }
    
    public override string Message { get; }
    
    public override Option<Error> Inner { get; }

    public override bool IsExceptional =>
        (int)this.StatusCode >= 500;
    
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

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(this.Code), this.Code);
        info.AddValue(nameof(this.Message), this.Message);
        info.AddValue(nameof(this.StatusCode), this.StatusCode);
        info.AddValue(nameof(this.ErrorCode), this.ErrorCode);
        info.AddValue(nameof(this.Inner), this.Inner);
    }
    
    public override bool Is<E>()
    {
        return this.Exception
                .Match(
                    Some: ex => ex is E,
                    None: () => false);
    }

    public override ErrorException ToErrorException()
    {
        return this.Exception
                .Match(
                    Some: ex => new ExceptionalException(ex),
                    None: () => new ExceptionalException(this.Message, this.Code));
    }

    public static HttpResultError New(
        HttpStatusCode statusCode,
        string message,
        Option<string> errorCode = default,
        Option<object> data = default,
        Option<Error> error = default)
    {

        return
            new HttpResultError(
                statusCode,
                message,
                errorCode,
                data,
                error);
    }
}