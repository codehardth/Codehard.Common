using System.Runtime.Serialization;

namespace Codehard.Common.DomainModel.Types;

/// <summary>
/// Represents a monetary value with a specified amount and currency.
/// </summary>
[Serializable]
public record Money : ISerializable
{
    /// <summary>
    /// Default constructor used by EF Core.
    /// </summary>
    public Money()
    {
        // EF Core Constructor.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Money"/> record with the specified amount and currency.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="currency">The currency of the amount.</param>
    public Money(decimal amount, Currency currency)
    {
        this.Amount = amount;
        this.Currency = currency;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Money"/> record with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> containing the serialized data.</param>
    /// <param name="context">The <see cref="StreamingContext"/> representing the source and destination of the serialized stream.</param>
    public Money(SerializationInfo info, StreamingContext context)
    {
        this.Amount = info.GetDecimal(nameof(this.Amount));
        this.Currency = (Currency)(info.GetValue(nameof(this.Currency), typeof(Currency)) ?? throw new InvalidOperationException());
    }

    /// <summary>
    /// Gets the monetary amount.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// Gets the currency of the amount.
    /// </summary>
    public Currency Currency { get; init; }

    /// <summary>
    /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the <see cref="Money"/> record.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
    /// <param name="context">The <see cref="StreamingContext"/> representing the source and destination of the serialized stream.</param>
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(this.Amount), this.Amount, typeof(decimal));
        info.AddValue(nameof(this.Currency), this.Currency, typeof(Currency));
    }
}