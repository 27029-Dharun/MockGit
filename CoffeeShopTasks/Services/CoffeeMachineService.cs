using CoffeeShopTasks.Models;

namespace CoffeeShopTasks.Services;

/// <summary>
/// Coffee machine service.
/// </summary>
internal class CoffeeMachineService
{
    private readonly Notify? _notifier;
    private int _machineAvailable;
    private Queue<Coffee> _coffees = new Queue<Coffee>();

    /// <summary>
    /// Initializes a new instance of the <see cref="CoffeeMachineService"/> class.
    /// </summary>
    /// <param name="notifier">notifier</param>
    /// <param name="machineAvailable">Machine count</param>
    public CoffeeMachineService(Notify? notifier, int machineAvailable)
    {
        this._notifier = notifier;
        this._machineAvailable = machineAvailable;
    }

    /// <summary>
    /// orders the coffee
    /// </summary>
    /// <param name="coffee">A coffee type to place an order.</param>
    /// <returns>A asynchronous task</returns>
    internal async Task OrderCoffee(Coffee coffee)
    {
        if (this._machineAvailable > 0)
        {
            this._machineAvailable--;
            this._notifier?.Execute($"Order placed {coffee.Name}");
            await Task.Delay(coffee.SourceTime);
            this._notifier?.Execute($"{coffee.Name} sourcing done");

            await Task.Delay(coffee.PreparationTime);
            this._notifier?.Execute($"{coffee.Name} was ready");

            this._machineAvailable++;
            this.ProcessNextOrder();
        }
        else
        {
            this._notifier?.Execute($"{coffee.Name} added to queue");
            this._coffees.Enqueue(coffee);
        }
    }

    private void ProcessNextOrder()
    {
        if (this._coffees.Count == 0)
        {
            return;
        }

        Task task = this.OrderCoffee(this._coffees.Dequeue());
    }
}
