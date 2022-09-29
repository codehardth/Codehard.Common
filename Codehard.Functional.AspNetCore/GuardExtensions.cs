namespace Codehard.Functional.AspNetCore;

public static class GuardExtensions
{
    public static Guard<Error> GuardWithOK(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.OK, message));
    }

    public static Guard<Error> GuardWithCreated(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.Created, message));
    }

    public static Guard<Error> GuardWithAccepted(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.Accepted, message));
    }

    public static Guard<Error> GuardWithNoContent(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.NoContent, message));
    }

    public static Guard<Error> GuardWithBadRequest(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.BadRequest, message));
    }

    public static Guard<Error> GuardWithUnauthorized(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.Unauthorized, message));
    }

    public static Guard<Error> GuardWithForbidden(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.Forbidden, message));
    }

    public static Guard<Error> GuardWithNotFound(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.NotFound, message));
    }

    public static Guard<Error> GuardWithConflict(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.Conflict, message));
    }

    public static Guard<Error> GuardWithUnprocessableEntity(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.UnprocessableEntity, message));
    }

    public static Guard<Error> GuardWithLocked(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.Locked, message));
    }

    public static Guard<Error> GuardWithInternalServerError(bool flag, string message = "")
    {
        return new Guard<Error>(
            flag,
            Error.New((int) HttpStatusCode.InternalServerError, message));
    }
}