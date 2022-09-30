// <auto-generated/>
namespace Codehard.Functional.AspNetCore;

public static class AsyncEffectExtensions
{
    public static Aff<A> MapFailToOK<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.OK, _ => errorMessage);

    public static Aff<A> MapFailToOK<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.OK, messageFunc);

    public static Aff<A> MapFailToCreated<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.Created, _ => errorMessage);

    public static Aff<A> MapFailToCreated<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.Created, messageFunc);

    public static Aff<A> MapFailToAccepted<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.Accepted, _ => errorMessage);

    public static Aff<A> MapFailToAccepted<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.Accepted, messageFunc);

    public static Aff<A> MapFailToNoContent<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.NoContent, _ => errorMessage);

    public static Aff<A> MapFailToNoContent<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.NoContent, messageFunc);

    public static Aff<A> MapFailToBadRequest<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.BadRequest, _ => errorMessage);

    public static Aff<A> MapFailToBadRequest<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.BadRequest, messageFunc);

    public static Aff<A> MapFailToUnauthorized<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.Unauthorized, _ => errorMessage);

    public static Aff<A> MapFailToUnauthorized<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.Unauthorized, messageFunc);

    public static Aff<A> MapFailToForbidden<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.Forbidden, _ => errorMessage);

    public static Aff<A> MapFailToForbidden<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.Forbidden, messageFunc);

    public static Aff<A> MapFailToNotFound<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.NotFound, _ => errorMessage);

    public static Aff<A> MapFailToNotFound<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.NotFound, messageFunc);

    public static Aff<A> MapFailToConflict<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.Conflict, _ => errorMessage);

    public static Aff<A> MapFailToConflict<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.Conflict, messageFunc);

    public static Aff<A> MapFailToUnprocessableEntity<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.UnprocessableEntity, _ => errorMessage);

    public static Aff<A> MapFailToUnprocessableEntity<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.UnprocessableEntity, messageFunc);

    public static Aff<A> MapFailToLocked<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.Locked, _ => errorMessage);

    public static Aff<A> MapFailToLocked<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.Locked, messageFunc);

    public static Aff<A> MapFailToInternalServerError<A>(this Aff<A> ma, string errorMessage = "")
        => ma.CustomError((int)HttpStatusCode.InternalServerError, _ => errorMessage);

    public static Aff<A> MapFailToInternalServerError<A>(this Aff<A> ma, Func<Error, string> messageFunc)
        => ma.CustomError((int)HttpStatusCode.InternalServerError, messageFunc);

    public static Aff<A> ToAffWithFailToOK<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToOK(_ => errorMessage);

    public static Aff<A> ToAffWithFailToOK<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToOK(errorMessageFunc);

    public static Aff<A> ToAffWithFailToCreated<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToCreated(_ => errorMessage);

    public static Aff<A> ToAffWithFailToCreated<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToCreated(errorMessageFunc);

    public static Aff<A> ToAffWithFailToAccepted<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToAccepted(_ => errorMessage);

    public static Aff<A> ToAffWithFailToAccepted<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToAccepted(errorMessageFunc);

    public static Aff<A> ToAffWithFailToNoContent<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToNoContent(_ => errorMessage);

    public static Aff<A> ToAffWithFailToNoContent<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToNoContent(errorMessageFunc);

    public static Aff<A> ToAffWithFailToBadRequest<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToBadRequest(_ => errorMessage);

    public static Aff<A> ToAffWithFailToBadRequest<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToBadRequest(errorMessageFunc);

    public static Aff<A> ToAffWithFailToUnauthorized<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToUnauthorized(_ => errorMessage);

    public static Aff<A> ToAffWithFailToUnauthorized<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToUnauthorized(errorMessageFunc);

    public static Aff<A> ToAffWithFailToForbidden<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToForbidden(_ => errorMessage);

    public static Aff<A> ToAffWithFailToForbidden<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToForbidden(errorMessageFunc);

    public static Aff<A> ToAffWithFailToNotFound<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToNotFound(_ => errorMessage);

    public static Aff<A> ToAffWithFailToNotFound<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToNotFound(errorMessageFunc);

    public static Aff<A> ToAffWithFailToConflict<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToConflict(_ => errorMessage);

    public static Aff<A> ToAffWithFailToConflict<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToConflict(errorMessageFunc);

    public static Aff<A> ToAffWithFailToUnprocessableEntity<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToUnprocessableEntity(_ => errorMessage);

    public static Aff<A> ToAffWithFailToUnprocessableEntity<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToUnprocessableEntity(errorMessageFunc);

    public static Aff<A> ToAffWithFailToLocked<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToLocked(_ => errorMessage);

    public static Aff<A> ToAffWithFailToLocked<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToLocked(errorMessageFunc);

    public static Aff<A> ToAffWithFailToInternalServerError<A>(this Task<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToInternalServerError(_ => errorMessage);

    public static Aff<A> ToAffWithFailToInternalServerError<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToInternalServerError(errorMessageFunc);

    public static Aff<A> ToAffWithFailToOK<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToOK(_ => errorMessage);

    public static Aff<A> ToAffWithFailToOK<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToOK(errorMessageFunc);

    public static Aff<A> ToAffWithFailToCreated<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToCreated(_ => errorMessage);

    public static Aff<A> ToAffWithFailToCreated<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToCreated(errorMessageFunc);

    public static Aff<A> ToAffWithFailToAccepted<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToAccepted(_ => errorMessage);

    public static Aff<A> ToAffWithFailToAccepted<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToAccepted(errorMessageFunc);

    public static Aff<A> ToAffWithFailToNoContent<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToNoContent(_ => errorMessage);

    public static Aff<A> ToAffWithFailToNoContent<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToNoContent(errorMessageFunc);

    public static Aff<A> ToAffWithFailToBadRequest<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToBadRequest(_ => errorMessage);

    public static Aff<A> ToAffWithFailToBadRequest<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToBadRequest(errorMessageFunc);

    public static Aff<A> ToAffWithFailToUnauthorized<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToUnauthorized(_ => errorMessage);

    public static Aff<A> ToAffWithFailToUnauthorized<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToUnauthorized(errorMessageFunc);

    public static Aff<A> ToAffWithFailToForbidden<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToForbidden(_ => errorMessage);

    public static Aff<A> ToAffWithFailToForbidden<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToForbidden(errorMessageFunc);

    public static Aff<A> ToAffWithFailToNotFound<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToNotFound(_ => errorMessage);

    public static Aff<A> ToAffWithFailToNotFound<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToNotFound(errorMessageFunc);

    public static Aff<A> ToAffWithFailToConflict<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToConflict(_ => errorMessage);

    public static Aff<A> ToAffWithFailToConflict<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToConflict(errorMessageFunc);

    public static Aff<A> ToAffWithFailToUnprocessableEntity<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToUnprocessableEntity(_ => errorMessage);

    public static Aff<A> ToAffWithFailToUnprocessableEntity<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToUnprocessableEntity(errorMessageFunc);

    public static Aff<A> ToAffWithFailToLocked<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToLocked(_ => errorMessage);

    public static Aff<A> ToAffWithFailToLocked<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToLocked(errorMessageFunc);

    public static Aff<A> ToAffWithFailToInternalServerError<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToInternalServerError(_ => errorMessage);

    public static Aff<A> ToAffWithFailToInternalServerError<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToInternalServerError(errorMessageFunc);
}