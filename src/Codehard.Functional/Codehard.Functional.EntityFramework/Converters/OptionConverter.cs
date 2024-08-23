using System.Linq.Expressions;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using static LanguageExt.Prelude;

namespace Codehard.Functional.EntityFramework.Converters;

public sealed class OptionConverter<T> : ValueConverter<Option<T>, T?>
{
    public OptionConverter()
        : base(
            opt => opt.ValueUnsafe(),
            v => Optional(v))
    {
    }

    public OptionConverter(
        Expression<Func<Option<T>, T>> convertToProviderExpression,
        Expression<Func<T, Option<T>>> convertFromProviderExpression,
        ConverterMappingHints? mappingHints = null)
        : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
    {
    }

    public OptionConverter(
        Expression<Func<Option<T>, T>> convertToProviderExpression,
        Expression<Func<T, Option<T>>> convertFromProviderExpression,
        bool convertsNulls,
        ConverterMappingHints? mappingHints = null)
        : base(convertToProviderExpression, convertFromProviderExpression, convertsNulls, mappingHints)
    {
    }
}