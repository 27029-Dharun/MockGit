using CoffeeShopApp.Enums;

namespace CoffeeShopApp.Models;

/// <summary>
/// Represents a drink offered by the coffee shop.
/// </summary>
public sealed class Coffee
{
    /// <summary>Gets the drink type.</summary>
    public CoffeeType Type { get; init; }

    /// <summary>Gets the display name.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Gets the price.</summary>
    public decimal Price { get; init; }

    /// <summary>Gets the sourcing duration in seconds.</summary>
    public int SourcingSeconds { get; init; }

    /// <summary>Gets the preparation duration in seconds.</summary>
    public int PreparationSeconds { get; init; }
}
