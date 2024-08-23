// <auto-generated/>
namespace Codehard.Functional.AspNetCore;

public static class TaskExtensions
{
    #region Task<A>

    public static Eff<A> ToEffWithFailToOK<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToOK(message);

    public static Eff<A> ToEffWithFailToOK<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToOK(errorMessageFunc);
    
    
    public static Eff<A> ToEffWithFailToCreated<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToCreated(message);

    public static Eff<A> ToEffWithFailToCreated<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToCreated(errorMessageFunc);


    public static Eff<A> ToEffWithFailToAccepted<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToAccepted(message);

    public static Eff<A> ToEffWithFailToAccepted<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToAccepted(errorMessageFunc);


    public static Eff<A> ToEffWithFailToNoContent<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToNoContent(message);

    public static Eff<A> ToEffWithFailToNoContent<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToNoContent(errorMessageFunc);


    public static Eff<A> ToEffWithFailToBadRequest<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToBadRequest(message);

    public static Eff<A> ToEffWithFailToBadRequest<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToBadRequest(errorMessageFunc);


    public static Eff<A> ToEffWithFailToUnauthorized<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToUnauthorized(message);

    public static Eff<A> ToEffWithFailToUnauthorized<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToUnauthorized(errorMessageFunc);


    public static Eff<A> ToEffWithFailToForbidden<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToForbidden(message);

    public static Eff<A> ToEffWithFailToForbidden<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToForbidden(errorMessageFunc);


    public static Eff<A> ToEffWithFailToNotFound<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToNotFound(message);

    public static Eff<A> ToEffWithFailToNotFound<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToNotFound(errorMessageFunc);


    public static Eff<A> ToEffWithFailToConflict<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToConflict(message);

    public static Eff<A> ToEffWithFailToConflict<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToConflict(errorMessageFunc);


    public static Eff<A> ToEffWithFailToUnprocessableEntity<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToUnprocessableEntity(message);

    public static Eff<A> ToEffWithFailToUnprocessableEntity<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToUnprocessableEntity(errorMessageFunc);


    public static Eff<A> ToEffWithFailToLocked<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToLocked(message);

    public static Eff<A> ToEffWithFailToLocked<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToLocked(errorMessageFunc);


    public static Eff<A> ToEffWithFailToInternalServerError<A>(this Task<A> ma, string message = "")
        => liftEff(() => ma).MapFailToInternalServerError(message);

    public static Eff<A> ToEffWithFailToInternalServerError<A>(this Task<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(() => ma).MapFailToInternalServerError(errorMessageFunc);

    #endregion
    
    #region ValueTask<A>

    public static Eff<A> ToEffWithFailToOK<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToOK(errorMessage);

    public static Eff<A> ToEffWithFailToOK<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToOK(errorMessageFunc);

    public static Eff<A> ToEffWithFailToCreated<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToCreated(errorMessage);

    public static Eff<A> ToEffWithFailToCreated<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToCreated(errorMessageFunc);

    public static Eff<A> ToEffWithFailToAccepted<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToAccepted(errorMessage);

    public static Eff<A> ToEffWithFailToAccepted<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToAccepted(errorMessageFunc);

    public static Eff<A> ToEffWithFailToNoContent<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToNoContent(errorMessage);

    public static Eff<A> ToEffWithFailToNoContent<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToNoContent(errorMessageFunc);

    public static Eff<A> ToEffWithFailToBadRequest<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToBadRequest(errorMessage);

    public static Eff<A> ToEffWithFailToBadRequest<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToBadRequest(errorMessageFunc);

    public static Eff<A> ToEffWithFailToUnauthorized<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToUnauthorized(errorMessage);

    public static Eff<A> ToEffWithFailToUnauthorized<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToUnauthorized(errorMessageFunc);

    public static Eff<A> ToEffWithFailToForbidden<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToForbidden(errorMessage);

    public static Eff<A> ToEffWithFailToForbidden<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToForbidden(errorMessageFunc);

    public static Eff<A> ToEffWithFailToNotFound<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToNotFound(errorMessage);

    public static Eff<A> ToEffWithFailToNotFound<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToNotFound(errorMessageFunc);

    public static Eff<A> ToEffWithFailToConflict<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToConflict(errorMessage);

    public static Eff<A> ToEffWithFailToConflict<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToConflict(errorMessageFunc);

    public static Eff<A> ToEffWithFailToUnprocessableEntity<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToUnprocessableEntity(errorMessage);

    public static Eff<A> ToEffWithFailToUnprocessableEntity<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToUnprocessableEntity(errorMessageFunc);

    public static Eff<A> ToEffWithFailToLocked<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToLocked(errorMessage);

    public static Eff<A> ToEffWithFailToLocked<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToLocked(errorMessageFunc);

    public static Eff<A> ToEffWithFailToInternalServerError<A>(this ValueTask<A> ma, string errorMessage = "")
        => liftEff(async () => await ma).MapFailToInternalServerError(errorMessage);

    public static Eff<A> ToEffWithFailToInternalServerError<A>(this ValueTask<A> ma, Func<Error, string> errorMessageFunc)
        => liftEff(async () => await ma).MapFailToInternalServerError(errorMessageFunc);

    #endregion

#nullable enable

    #region Task<A?>

    public static Eff<Option<A>> ToEffOptionWithFailToOK<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToOK(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToOK<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToOK(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToCreated<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToCreated(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToCreated<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToCreated(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToAccepted<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToAccepted(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToAccepted<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToAccepted(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToNoContent<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToNoContent(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToNoContent<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToNoContent(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToBadRequest<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToBadRequest(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToBadRequest<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToBadRequest(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToUnauthorized<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToUnauthorized(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToUnauthorized<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToUnauthorized(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToForbidden<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToForbidden(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToForbidden<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToForbidden(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToNotFound<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToNotFound(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToNotFound<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToNotFound(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToConflict<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToConflict(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToConflict<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToConflict(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToUnprocessableEntity<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToUnprocessableEntity(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToUnprocessableEntity<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToUnprocessableEntity(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToLocked<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToLocked(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToLocked<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToLocked(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToInternalServerError<A>(this Task<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToInternalServerError(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToInternalServerError<A>(this Task<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToInternalServerError(errorMessageFunc);

    #endregion

    #region ValueTask<A?>

    public static Eff<Option<A>> ToEffOptionWithFailToOK<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToOK(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToOK<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToOK(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToCreated<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToCreated(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToCreated<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToCreated(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToAccepted<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToAccepted(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToAccepted<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToAccepted(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToNoContent<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToNoContent(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToNoContent<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToNoContent(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToBadRequest<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToBadRequest(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToBadRequest<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToBadRequest(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToUnauthorized<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToUnauthorized(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToUnauthorized<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToUnauthorized(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToForbidden<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToForbidden(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToForbidden<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToForbidden(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToNotFound<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToNotFound(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToNotFound<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToNotFound(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToConflict<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToConflict(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToConflict<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToConflict(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToUnprocessableEntity<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToUnprocessableEntity(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToUnprocessableEntity<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToUnprocessableEntity(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToLocked<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToLocked(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToLocked<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToLocked(errorMessageFunc);

    public static Eff<Option<A>> ToEffOptionWithFailToInternalServerError<A>(this ValueTask<A?> ma, string errorMessage = "")
        => ma.ToEffOption().MapFailToInternalServerError(errorMessage);

    public static Eff<Option<A>> ToEffOptionWithFailToInternalServerError<A>(this ValueTask<A?> ma, Func<Error, string> errorMessageFunc)
        => ma.ToEffOption().MapFailToInternalServerError(errorMessageFunc);

    #endregion

#nullable disable
}