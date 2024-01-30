namespace Codehard.Functional.AspNetCore;

public static class Commons
{
    internal static IActionResult MapToActionResult<T>(HttpStatusCode statusCode, T result)
    {
        return
            result switch
            {
                IActionResult ar => ar,
                Unit => new StatusCodeResult((int)statusCode),
                _ => new ObjectResult(result)
                {
                    StatusCode = (int)statusCode,
                },
            };
    }
}