using CoffeeShopApp.Repositories;
using CoffeeShopApp.Services;
using CoffeeShopApp.Views;

namespace CoffeeShopApp;

/// <summary>
/// Application entry point.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Starts the coffee shop application.
    /// </summary>
    private static void Main()
    {
        string dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDirectory);

        INotificationRepository notificationRepository =
            new JsonNotificationRepository(
                Path.Combine(dataDirectory, "notifications.json"));

        var menuService = new MenuService();
        var notificationService =
            new NotificationService(notificationRepository);

        var machineService = new CoffeeMachineService(
            machineCount: 3,
            menuService,
            notificationService);

        using var notificationWorker =
            new NotificationWorker(notificationService);

        var consoleView = new ConsoleView(
            menuService,
            machineService,
            notificationRepository);

        notificationWorker.Start();

        try
        {
            consoleView.Run();
        }
        finally
        {
            machineService.Stop();
        }
    }
}
