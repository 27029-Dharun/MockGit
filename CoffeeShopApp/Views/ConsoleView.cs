using CoffeeShopApp.Enums;
using CoffeeShopApp.Models;
using CoffeeShopApp.Repositories;
using CoffeeShopApp.Services;

namespace CoffeeShopApp.Views;

/// <summary>
/// Provides the console user interface.
/// </summary>
public sealed class ConsoleView
{
    private readonly MenuService _menuService;
    private readonly CoffeeMachineService _machineService;
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Initializes the console view.
    /// </summary>
    public ConsoleView(
        MenuService menuService,
        CoffeeMachineService machineService,
        INotificationRepository notificationRepository)
    {
        _menuService = menuService;
        _machineService = machineService;
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Runs the interactive menu.
    /// </summary>
    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("         COFFEE SHOP MACHINE");
            Console.WriteLine("======================================");
            Console.WriteLine("1. Show Menu");
            Console.WriteLine("2. Place Order");
            Console.WriteLine("3. Show Machines");
            Console.WriteLine("4. Show Notifications");
            Console.WriteLine("0. Exit");
            Console.Write("Choose: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowMenu();
                    break;
                case "2":
                    PlaceOrder();
                    break;
                case "3":
                    ShowMachines();
                    break;
                case "4":
                    ShowNotifications();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine();

        foreach (Coffee coffee in _menuService.GetAll())
        {
            Console.WriteLine(
                $"{(int)coffee.Type}. {coffee.Name,-12} " +
                $"₹{coffee.Price,6:0.00} | " +
                $"Source: {coffee.SourcingSeconds}s | " +
                $"Prepare: {coffee.PreparationSeconds}s");
        }
    }

    private void PlaceOrder()
    {
        ShowMenu();
        Console.Write("Enter drink number: ");

        if (!int.TryParse(Console.ReadLine(), out int value) ||
            !Enum.IsDefined(typeof(CoffeeType), value))
        {
            Console.WriteLine("Invalid drink selection.");
            return;
        }

        int? orderId = _machineService.PlaceOrder((CoffeeType)value);

        Console.WriteLine(
            orderId.HasValue
                ? $"Order #{orderId.Value} accepted."
                : "Order could not be accepted.");
    }

    private void ShowMachines()
    {
        Console.WriteLine();

        foreach (CoffeeMachine machine in _machineService.GetMachines())
        {
            string status = machine.IsBusy
                ? $"Busy - Order #{machine.CurrentOrderId} ({machine.CurrentCoffee})"
                : "Available";

            Console.WriteLine($"Machine {machine.Id}: {status}");
        }
    }

    private void ShowNotifications()
    {
        Console.WriteLine();
        IReadOnlyList<Notification> notifications =
            _notificationRepository.GetAll();

        if (notifications.Count == 0)
        {
            Console.WriteLine("No notifications found.");
            return;
        }

        foreach (Notification notification in notifications)
        {
            Console.WriteLine(
                $"{notification.CreatedAt:HH:mm:ss} | " +
                $"{notification.Type,-7} | {notification.Message}");
        }
    }
}
