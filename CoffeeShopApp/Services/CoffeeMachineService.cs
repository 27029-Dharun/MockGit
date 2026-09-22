using CoffeeShopApp.Enums;
using CoffeeShopApp.Models;

namespace CoffeeShopApp.Services;

/// <summary>
/// Coordinates three coffee machines and processes orders concurrently.
/// </summary>
public sealed class CoffeeMachineService
{
    private readonly List<CoffeeMachine> _machines;
    private readonly MenuService _menuService;
    private readonly NotificationService _notificationService;
    private readonly object _lock = new();
    private readonly List<Thread> _orderThreads = [];
    private int _nextOrderId;
    private bool _stopping;

    /// <summary>
    /// Initializes the machine service.
    /// </summary>
    /// <param name="machineCount">Number of machines.</param>
    /// <param name="menuService">Menu service.</param>
    /// <param name="notificationService">Notification service.</param>
    public CoffeeMachineService(
        int machineCount,
        MenuService menuService,
        NotificationService notificationService)
    {
        if (machineCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(machineCount));
        }

        _menuService = menuService;
        _notificationService = notificationService;

        _machines = Enumerable.Range(1, machineCount)
            .Select(id => new CoffeeMachine { Id = id })
            .ToList();
    }

    /// <summary>
    /// Places an order on an available machine.
    /// </summary>
    /// <param name="coffeeType">Requested drink.</param>
    /// <returns>Order identifier, or null when rejected.</returns>
    public int? PlaceOrder(CoffeeType coffeeType)
    {
        Coffee? coffee = _menuService.GetByType(coffeeType);

        if (coffee is null)
        {
            _notificationService.Publish(
                NotificationType.Error,
                $"Drink '{coffeeType}' is not available.");

            return null;
        }

        int orderId;
        int machineId;

        lock (_lock)
        {
            if (_stopping)
            {
                return null;
            }

            CoffeeMachine? machine =
                _machines.FirstOrDefault(item => !item.IsBusy);

            if (machine is null)
            {
                _notificationService.Publish(
                    NotificationType.Machine,
                    "All machines are busy. Order was not accepted.");

                return null;
            }

            orderId = ++_nextOrderId;
            machineId = machine.Id;

            machine.IsBusy = true;
            machine.CurrentOrderId = orderId;
            machine.CurrentCoffee = coffeeType;

            var thread = new Thread(
                () => ProcessOrder(orderId, machineId, coffee))
            {
                IsBackground = true,
                Name = $"Order-{orderId}-Machine-{machineId}"
            };

            _orderThreads.Add(thread);
            thread.Start();
        }

        _notificationService.Publish(
            NotificationType.Order,
            $"Order #{orderId} accepted: {coffee.Name} " +
            $"using Machine {machineId}.");

        return orderId;
    }

    /// <summary>
    /// Gets a snapshot of machine states.
    /// </summary>
    /// <returns>Current machine state snapshot.</returns>
    public IReadOnlyList<CoffeeMachine> GetMachines()
    {
        lock (_lock)
        {
            return _machines
                .Select(machine => new CoffeeMachine
                {
                    Id = machine.Id,
                    IsBusy = machine.IsBusy,
                    CurrentOrderId = machine.CurrentOrderId,
                    CurrentCoffee = machine.CurrentCoffee
                })
                .ToList()
                .AsReadOnly();
        }
    }

    /// <summary>
    /// Stops accepting orders and waits for active orders to finish.
    /// </summary>
    public void Stop()
    {
        List<Thread> threads;

        lock (_lock)
        {
            _stopping = true;
            threads = _orderThreads.ToList();
        }

        foreach (Thread thread in threads)
        {
            if (thread.IsAlive)
            {
                thread.Join();
            }
        }
    }

    private void ProcessOrder(
        int orderId,
        int machineId,
        Coffee coffee)
    {
        try
        {
            _notificationService.Publish(
                NotificationType.Order,
                $"Order #{orderId}: sourcing {coffee.Name}.");

            Thread.Sleep(TimeSpan.FromSeconds(coffee.SourcingSeconds));

            _notificationService.Publish(
                NotificationType.Order,
                $"Order #{orderId}: preparing {coffee.Name} " +
                $"on Machine {machineId}.");

            Thread.Sleep(TimeSpan.FromSeconds(coffee.PreparationSeconds));

            _notificationService.Publish(
                NotificationType.Order,
                $"Order #{orderId} completed. " +
                $"{coffee.Name} is ready.");

            lock (_lock)
            {
                CoffeeMachine machine =
                    _machines.Single(item => item.Id == machineId);

                machine.IsBusy = false;
                machine.CurrentOrderId = null;
                machine.CurrentCoffee = null;
            }

            _notificationService.Publish(
                NotificationType.Machine,
                $"Machine {machineId} is available.");
        }
        catch (Exception ex)
        {
            lock (_lock)
            {
                CoffeeMachine machine =
                    _machines.Single(item => item.Id == machineId);

                machine.IsBusy = false;
                machine.CurrentOrderId = null;
                machine.CurrentCoffee = null;
            }

            _notificationService.Publish(
                NotificationType.Error,
                $"Order #{orderId} failed: {ex.Message}");
        }
    }
}
