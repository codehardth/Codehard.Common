using System.Text.Json.Serialization;
using Codehard.Common.DomainModel.Converters;

namespace Codehard.Common.DomainModel;

public interface IEntityKey
{
}

public interface IEntity<TKey>
    where TKey : IEntityKey
{
    TKey Id { get; }
}

[JsonConverter(typeof(IntegerKeyJsonConverter))]
public record struct IntegerKey(int Value) 
    : IEntityKey, IComparable<IntegerKey>
{
    public int CompareTo(IntegerKey other)
    {
        return this.Value.CompareTo(other.Value);
    }
    
    public static bool operator >(IntegerKey l, IntegerKey r)
    {
        return l.CompareTo(r) > 0;
    }
    
    public static bool operator <(IntegerKey l, IntegerKey r)
    {
        return l.CompareTo(r) < 0;
    }
    
    public static bool operator >=(IntegerKey l, IntegerKey r)
    {
        return l.CompareTo(r) >= 0;
    }
    
    public static bool operator <=(IntegerKey l, IntegerKey r)
    {
        return l.CompareTo(r) <= 0;
    }
    
    public static implicit operator IntegerKey(int b) => new(b);
}

[JsonConverter(typeof(LongKeyJsonConverter))]
public record struct LongKey(long Value)
    : IEntityKey, IComparable<LongKey>
{
    public int CompareTo(LongKey other)
    {
        return this.Value.CompareTo(other.Value);
    }
    
    public static bool operator >(LongKey l, LongKey r)
    {
        return l.CompareTo(r) > 0;
    }
    
    public static bool operator <(LongKey l, LongKey r)
    {
        return l.CompareTo(r) < 0;
    }
    
    public static bool operator >=(LongKey l, LongKey r)
    {
        return l.CompareTo(r) >= 0;
    }
    
    public static bool operator <=(LongKey l, LongKey r)
    {
        return l.CompareTo(r) <= 0;
    }
    
    public static implicit operator LongKey(long b) => new(b);
}

[JsonConverter(typeof(StringKeyJsonConverter))]
public record struct StringKey(string Value)
    : IEntityKey, IComparable<StringKey>
{
    public int CompareTo(StringKey other)
    {
        return this.Value.CompareTo(other.Value);
    }
    
    public static bool operator >(StringKey l, StringKey r)
    {
        return l.CompareTo(r) > 0;
    }
    
    public static bool operator <(StringKey l, StringKey r)
    {
        return l.CompareTo(r) < 0;
    }
    
    public static bool operator >=(StringKey l, StringKey r)
    {
        return l.CompareTo(r) >= 0;
    }
    
    public static bool operator <=(StringKey l, StringKey r)
    {
        return l.CompareTo(r) <= 0;
    }
    
    public static implicit operator StringKey(string b) => new(b);
}

[JsonConverter(typeof(GuidKeyJsonConverter))]
public record struct GuidKey(Guid Value)
    : IEntityKey, IComparable<GuidKey>
{
    public int CompareTo(GuidKey other)
    {
        return this.Value.CompareTo(other.Value);
    }
    
    public static bool operator >(GuidKey l, GuidKey r)
    {
        return l.CompareTo(r) > 0;
    }
    
    public static bool operator <(GuidKey l, GuidKey r)
    {
        return l.CompareTo(r) < 0;
    }
    
    public static bool operator >=(GuidKey l, GuidKey r)
    {
        return l.CompareTo(r) >= 0;
    }
    
    public static bool operator <=(GuidKey l, GuidKey r)
    {
        return l.CompareTo(r) <= 0;
    }
    
    public static implicit operator GuidKey(Guid b) => new(b);
};