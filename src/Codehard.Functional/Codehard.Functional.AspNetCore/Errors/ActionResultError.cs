namespace Codehard.Functional.AspNetCore.Errors;

/// <summary>
/// An HTTP error object.
/// </summary>
public record ActionResultError : Error
{
    private readonly IActionResult actionResult;
    
    private ActionResultError(IActionResult actionResult)
    {
        this.actionResult = actionResult;
        Message = string.Empty;
    }

    public override bool Is<E>() => false;

    public override ErrorException ToErrorException()
    {
        throw new NotSupportedException();
    }

    public override string Message { get; }
    
    public override bool IsExceptional => false;

    public override bool IsExpected => true;

    public IActionResult ActionResult => actionResult;
    
    public static ActionResultError New(
        IActionResult actionResult)
    {
        return new ActionResultError(actionResult);
    }
}