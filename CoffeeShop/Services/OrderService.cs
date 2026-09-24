using System.Collections.Concurrent;
using CoffeeShop.Models;
using CoffeeShop.Repository;

namespace CoffeeShop.Services;

internal class OrderService
{
    private readonly NotificationService? _notificationService;
    private int _availableMachineCount;
    private ConcurrentQueue<Order> _orderQueue = new();
    private readonly Logger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryService"/> class.
    /// </summary>
    /// <param name="notifier">notifier</param>
    /// <param name="machineAvailable">Machine count</param>
    public OrderService(NotificationService? notifier, int machineAvailable, Logger logger)
    {
        _notificationService = notifier;
        _availableMachineCount = machineAvailable;
        _logger = logger;
    }

    public async Task SubmitOrder(Order order)
    {
        if (_availableMachineCount > 0)
        {
            Interlocked.Decrement(ref _availableMachineCount);

            await ProcessOrder(order);
            return;
        }

        _orderQueue.Enqueue(order);

        _notificationService?.Execute($"{order.Coffee.Name} added to queue", order.UserId);
        _logger.LogText($"{order.Coffee.Name} added to queue {order.UserId}\n");
    }


    /// <summary>
    /// orders the coffee
    /// </summary>
    /// <param name="order">A coffee type to place an order.</param>
    /// <returns>A asynchronous task</returns>
    internal async Task ProcessOrder(Order order)
    {
        _notificationService?.Execute($"Started preparing {order.Coffee.Name} for User id: {order.UserId}", order.UserId);
        _logger.LogText($"Started preparing {order.Coffee.Name} for User id: {order.UserId}\n");

        await Task.Delay(order.Coffee.PreparationTime);
        _notificationService?.Execute($"{order.Coffee.Name} was ready", order.UserId);
        _logger.LogText($"{order.Coffee.Name} was ready {order.UserId}\n");

        Interlocked.Increment(ref _availableMachineCount);
        await ProcessNextOrder();
    }

    private async Task ProcessNextOrder()
    {
        if(!_orderQueue.Any())
        {
            return;
        }

        Order? nextOrder;
        while (!_orderQueue.TryDequeue(out nextOrder))
        {
        }

        Interlocked.Decrement(ref _availableMachineCount);
        await ProcessOrder(nextOrder);
    }
}
