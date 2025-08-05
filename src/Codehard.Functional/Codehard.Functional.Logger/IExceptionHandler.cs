using LanguageExt.Common;

namespace Codehard.Functional.Logger;

public interface IExceptionHandler
{
    Unit Handle(Error error, ILogger logger, LogLevel logLevel = LogLevel.Information);
}