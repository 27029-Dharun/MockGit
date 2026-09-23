using CoffeeShop.Controllers;
using CoffeeShop.Repository;
using CoffeeShop.Services;
using CoffeeShop.Views;

namespace CoffeeShop;

internal class Program
{
    private static async Task Main()
    {
        ConsoleView view = new();
        NotificationService notificationService = new();
        CoffeeRepository coffeeRepository = new CoffeeRepository();
        Inventory inventory = new Inventory();
        OrderService coffeeMachineService = new OrderService(notificationService, 3);
        InventoryService machineService = new(coffeeRepository, inventory, notificationService);
        CoffeeMachineController machineController = new(view, machineService, notificationService, coffeeMachineService);

        UserRepository repository = new UserRepository();
        UserService service = new UserService(repository);
        UserController controller = new UserController(service, view, machineController);

        await controller.Authenticate();
    }
}