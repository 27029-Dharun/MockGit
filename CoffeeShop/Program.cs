using CoffeeShop.Controllers;
using CoffeeShop.Repository;
using CoffeeShop.Services;
using CoffeeShop.Views;

namespace CoffeeShop;

internal class Program
{
    private static void Main()
    {
        Logger logger = new Logger("log.txt");

        NotificationService notificationService = new();

        ConsoleView view = new();
        CoffeeRepository coffeeRepository = new CoffeeRepository();
        Inventory inventory = new Inventory();
        MachineRepository machineRepository = new MachineRepository();
        MachineService machineService = new MachineService(machineRepository);

        OrderService coffeeMachineService = new OrderService(notificationService, machineService);
        InventoryService inventoryService = new(coffeeRepository, inventory, notificationService);
        CoffeeMachineController machineController = new(view, inventoryService, notificationService, coffeeMachineService, logger);

        UserRepository repository = new UserRepository();
        UserService service = new UserService(repository);
        UserController controller = new UserController(service, view, machineController);

        controller.Authenticate();
    }
}