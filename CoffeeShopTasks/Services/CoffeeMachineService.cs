using CoffeeShopTasks.Models;

namespace CoffeeShopTasks.Services;

/// <summary>
/// Coffee machine service.
/// </summary>
internal class CoffeeMachineService
{
    private readonly Notify? _notifier;

    public CoffeeMachineService(Notify? notifier)
    {
        this._notifier = notifier;
    }

    /// <summary>
    /// orders the coffee
    /// </summary>
    /// <param name="coffee">A coffee type to place an order.</param>
    /// <returns>A asynchronous task</returns>
    internal async Task OrderCoffee(Coffee coffee)
    {
        this._notifier?.Execute($"Order placed {coffee.Name}");
        await Task.Delay(coffee.SourceTime);
        this._notifier?.Execute($"{coffee.Name} sourcing done");

        await Task.Delay(coffee.PreparationTime);
        this._notifier?.Execute($"{coffee.Name} was ready");
    }
}
