using System.Runtime.Serialization;

namespace Codehard.Common.DomainModel.Types;

[Serializable]
public record Money : ISerializable
{
    public Money()
    {
        // EF Core Constructor.
    }

    public Money(decimal amount, Currency currency)
    {
        this.Amount = amount;
        this.Currency = currency;
    }

    public Money(SerializationInfo info, StreamingContext context)
    {
        this.Amount = info.GetDecimal(nameof(this.Amount));
        this.Currency = (Currency)(info.GetValue(nameof(this.Currency), typeof(Currency)) ?? throw new InvalidOperationException());
    }

    public decimal Amount { get; init; }

    public Currency Currency { get; init; }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(this.Amount), this.Amount, typeof(decimal));
        info.AddValue(nameof(this.Currency), this.Currency, typeof(Currency));
    }
}