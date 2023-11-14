using LanguageExt;

using static LanguageExt.Prelude;

// ReSharper disable once CheckNamespace
namespace Marten;

public static class DocumentSessionExtensions
{
    public static Aff<Unit> SaveChangesAff(this IDocumentSession documentSession)
    {
        return Aff(async () => await documentSession.SaveChangesAsync().ToUnit());
    }
}