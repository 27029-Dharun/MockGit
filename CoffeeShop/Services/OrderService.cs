using System.Collections.Concurrent;
using CoffeeShop.Models;

namespace CoffeeShop.Services;

internal class OrderService
{
    private readonly NotificationService? _notificationService;
    private readonly MachineService _machineService;
    private ConcurrentQueue<Order> _orderQueue = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryService"/> class.
    /// </summary>
    /// <param name="notifier">notifier</param>
    /// <param name="machineService">Machine count</param>
    public OrderService(NotificationService? notifier, MachineService machineService)
    {
        _notificationService = notifier;
        _machineService = machineService;
    }

    public void SubmitOrder(Order order)
    {
        CoffeeMachine? machine = this._machineService.AssignOrder(order);

        if (machine is null)
        {
            _orderQueue.Enqueue(order);

            _notificationService?.Execute($"{order.Coffee.Name} added to queue", order.UserId);
            return;
        }

        Task.Run(() => ProcessOrder(order, machine));
    }

    public async Task ProcessOrder(Order order, CoffeeMachine machine)
    {
        _notificationService?.Execute($"{order.Coffee.Name} started preparing", order.UserId);
        await Task.Delay(order.Coffee.PreparationTime);

        _notificationService?.Execute($"{order.Coffee.Name} ready", order.UserId);

        this._machineService.ReleaseOrder(machine);

        ProcessNextOrder();
    }

    private void ProcessNextOrder()
    {
        Order? order = GetNextOrder();
        if(order is null)
        {
            return;
        }

        CoffeeMachine? machine = this._machineService.AssignOrder(order);
        if(machine is null)
        {
            return;
        }

        Task.Run(() => ProcessOrder(order, machine));
    }

    private Order? GetNextOrder()
    {
        if (_orderQueue.TryDequeue(out Order? order))
        {
            return order;
        }

        return null;
    }
}
