// <auto-generated/>
namespace Codehard.Functional.AspNetCore;

public static class TaskExtensions
{
    #region Task<A>

    public static Aff<A> ToAffWithFailToOK<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToOK(message);

    public static Aff<A> ToAffWithFailToOK<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToOK(errorMessageFunc);


    public static Aff<A> ToAffWithFailToCreated<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToCreated(message);

    public static Aff<A> ToAffWithFailToCreated<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToCreated(errorMessageFunc);


    public static Aff<A> ToAffWithFailToAccepted<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToAccepted(message);

    public static Aff<A> ToAffWithFailToAccepted<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToAccepted(errorMessageFunc);


    public static Aff<A> ToAffWithFailToNoContent<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToNoContent(message);

    public static Aff<A> ToAffWithFailToNoContent<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToNoContent(errorMessageFunc);


    public static Aff<A> ToAffWithFailToBadRequest<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToBadRequest(message);

    public static Aff<A> ToAffWithFailToBadRequest<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToBadRequest(errorMessageFunc);


    public static Aff<A> ToAffWithFailToUnauthorized<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToUnauthorized(message);

    public static Aff<A> ToAffWithFailToUnauthorized<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToUnauthorized(errorMessageFunc);


    public static Aff<A> ToAffWithFailToForbidden<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToForbidden(message);

    public static Aff<A> ToAffWithFailToForbidden<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToForbidden(errorMessageFunc);


    public static Aff<A> ToAffWithFailToNotFound<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToNotFound(message);

    public static Aff<A> ToAffWithFailToNotFound<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToNotFound(errorMessageFunc);


    public static Aff<A> ToAffWithFailToConflict<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToConflict(message);

    public static Aff<A> ToAffWithFailToConflict<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToConflict(errorMessageFunc);


    public static Aff<A> ToAffWithFailToUnprocessableEntity<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToUnprocessableEntity(message);

    public static Aff<A> ToAffWithFailToUnprocessableEntity<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToUnprocessableEntity(errorMessageFunc);


    public static Aff<A> ToAffWithFailToLocked<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToLocked(message);

    public static Aff<A> ToAffWithFailToLocked<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToLocked(errorMessageFunc);


    public static Aff<A> ToAffWithFailToInternalServerError<A>(this Task<A> ma, string message = "")
        => ma.ToAff().MapFailToInternalServerError(message);

    public static Aff<A> ToAffWithFailToInternalServerError<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToInternalServerError(errorMessageFunc);

    #endregion
    
    #region ValueTask<A>

    public static Aff<A> ToAffWithFailToOK<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToOK(errorMessage);

    public static Aff<A> ToAffWithFailToOK<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToOK(errorMessageFunc);

    public static Aff<A> ToAffWithFailToCreated<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToCreated(errorMessage);

    public static Aff<A> ToAffWithFailToCreated<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToCreated(errorMessageFunc);

    public static Aff<A> ToAffWithFailToAccepted<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToAccepted(errorMessage);

    public static Aff<A> ToAffWithFailToAccepted<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToAccepted(errorMessageFunc);

    public static Aff<A> ToAffWithFailToNoContent<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToNoContent(errorMessage);

    public static Aff<A> ToAffWithFailToNoContent<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToNoContent(errorMessageFunc);

    public static Aff<A> ToAffWithFailToBadRequest<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToBadRequest(errorMessage);

    public static Aff<A> ToAffWithFailToBadRequest<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToBadRequest(errorMessageFunc);

    public static Aff<A> ToAffWithFailToUnauthorized<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToUnauthorized(errorMessage);

    public static Aff<A> ToAffWithFailToUnauthorized<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToUnauthorized(errorMessageFunc);

    public static Aff<A> ToAffWithFailToForbidden<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToForbidden(errorMessage);

    public static Aff<A> ToAffWithFailToForbidden<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToForbidden(errorMessageFunc);

    public static Aff<A> ToAffWithFailToNotFound<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToNotFound(errorMessage);

    public static Aff<A> ToAffWithFailToNotFound<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToNotFound(errorMessageFunc);

    public static Aff<A> ToAffWithFailToConflict<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToConflict(errorMessage);

    public static Aff<A> ToAffWithFailToConflict<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToConflict(errorMessageFunc);

    public static Aff<A> ToAffWithFailToUnprocessableEntity<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToUnprocessableEntity(errorMessage);

    public static Aff<A> ToAffWithFailToUnprocessableEntity<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToUnprocessableEntity(errorMessageFunc);

    public static Aff<A> ToAffWithFailToLocked<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToLocked(errorMessage);

    public static Aff<A> ToAffWithFailToLocked<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToLocked(errorMessageFunc);

    public static Aff<A> ToAffWithFailToInternalServerError<A>(this ValueTask<A> ma, string errorMessage = "")
        => ma.ToAff().MapFailToInternalServerError(errorMessage);

    public static Aff<A> ToAffWithFailToInternalServerError<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAff().MapFailToInternalServerError(errorMessageFunc);

    #endregion

#nullable enable

    #region Task<A?>

    public static Aff<Option<A>> ToAffOptionWithFailToOK<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToOK(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToOK<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToOK(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToCreated<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToCreated(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToCreated<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToCreated(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToAccepted<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToAccepted(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToAccepted<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToAccepted(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToNoContent<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToNoContent(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToNoContent<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToNoContent(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToBadRequest<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToBadRequest(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToBadRequest<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToBadRequest(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToUnauthorized<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToUnauthorized(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToUnauthorized<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToUnauthorized(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToForbidden<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToForbidden(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToForbidden<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToForbidden(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToNotFound<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToNotFound(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToNotFound<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToNotFound(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToConflict<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToConflict(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToConflict<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToConflict(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToUnprocessableEntity<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToUnprocessableEntity(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToUnprocessableEntity<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToUnprocessableEntity(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToLocked<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToLocked(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToLocked<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToLocked(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToInternalServerError<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToInternalServerError(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToInternalServerError<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToInternalServerError(errorMessageFunc);

    #endregion

    #region ValueTask<A?>

    public static Aff<Option<A>> ToAffOptionWithFailToOK<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToOK(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToOK<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToOK(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToCreated<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToCreated(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToCreated<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToCreated(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToAccepted<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToAccepted(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToAccepted<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToAccepted(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToNoContent<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToNoContent(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToNoContent<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToNoContent(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToBadRequest<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToBadRequest(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToBadRequest<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToBadRequest(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToUnauthorized<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToUnauthorized(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToUnauthorized<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToUnauthorized(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToForbidden<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToForbidden(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToForbidden<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToForbidden(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToNotFound<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToNotFound(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToNotFound<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToNotFound(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToConflict<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToConflict(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToConflict<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToConflict(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToUnprocessableEntity<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToUnprocessableEntity(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToUnprocessableEntity<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToUnprocessableEntity(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToLocked<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToLocked(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToLocked<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToLocked(errorMessageFunc);

    public static Aff<Option<A>> ToAffOptionWithFailToInternalServerError<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToAffOption().MapFailToInternalServerError(errorMessage);

    public static Aff<Option<A>> ToAffOptionWithFailToInternalServerError<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToAffOption().MapFailToInternalServerError(errorMessageFunc);

    #endregion

#nullable disable
}