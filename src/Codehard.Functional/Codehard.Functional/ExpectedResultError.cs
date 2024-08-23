// ReSharper disable InconsistentNaming
#pragma warning disable CS1591

namespace Codehard.Functional;

public record ExpectedResultError : Expected
{
    public ExpectedResultError(object errorObject, Error? inner)
        : base(string.Empty, 0, Optional(inner))
    {
        this.ErrorObject = errorObject;
    }

    public ExpectedResultError(object errorObject, Exception? exception = null)
        : base(string.Empty, 0, exception == null ? Option<Error>.None : Option<Error>.Some(ErrorException.New(exception)))
    {
        this.ErrorObject = errorObject;
    }

    public override ErrorException ToErrorException() => ErrorException.New(this.ErrorObject.ToString() ?? string.Empty);

    public override string ToString() => this.ErrorObject.ToString() ?? string.Empty;

    public object ErrorObject { get; init; }

    public void Deconstruct(out object errorObject)
    {
        errorObject = this.ErrorObject;
    }
}