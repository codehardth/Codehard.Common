namespace Codehard.Functional.AspNetCore;

public static class GuardExtensions
{
    public static Guard<LanguageExt.Common.Error> GuardWithOK(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.OK, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithCreated(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.Created, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithAccepted(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.Accepted, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithNoContent(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.NoContent, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithBadRequest(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.BadRequest, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithUnauthorized(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.Unauthorized, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithForbidden(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.Forbidden, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithNotFound(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.NotFound, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithConflict(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.Conflict, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithUnprocessableEntity(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.UnprocessableEntity, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithLocked(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.Locked, message));
    }

    public static Guard<LanguageExt.Common.Error> GuardWithInternalServerError(bool flag, string message = "")
    {
        return new Guard<LanguageExt.Common.Error>(
            flag,
            LanguageExt.Common.Error.New((int) HttpStatusCode.InternalServerError, message));
    }
}