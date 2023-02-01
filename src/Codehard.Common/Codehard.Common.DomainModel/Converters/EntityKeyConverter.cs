using System.ComponentModel;
using System.Globalization;

namespace Codehard.Common.DomainModel.Converters;

public sealed class EntityKeyConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(int)
               || sourceType == typeof(long)
               || sourceType == typeof(string)
               || sourceType == typeof(Guid)
               || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return value switch
        {
            int i => new IntegerKey(i),
            long l => new LongKey(l),
            string s => new StringKey(s),
            Guid g => new GuidKey(g),
            _ => base.ConvertFrom(context, culture, value)
        };
    }
}