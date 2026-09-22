using CoffeeShopApp.Enums;

namespace CoffeeShopApp.Models;

/// <summary>
/// Represents one coffee vending machine.
/// </summary>
public sealed class CoffeeMachine
{
    /// <summary>Gets the machine identifier.</summary>
    public int Id { get; init; }

    /// <summary>Gets or sets whether the machine is processing an order.</summary>
    public bool IsBusy { get; set; }

    /// <summary>Gets or sets the current order identifier.</summary>
    public int? CurrentOrderId { get; set; }

    /// <summary>Gets or sets the current drink.</summary>
    public CoffeeType? CurrentCoffee { get; set; }
}
