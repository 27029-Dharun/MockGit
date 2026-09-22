using CoffeeShopTasks.Controllers;
using CoffeeShopTasks.Services;
using CoffeeShopTasks.Views;

namespace Assignments;

internal class Program
{
    private static void Main(string[] args)
    {
        ConsoleView view = new ConsoleView();
        Notify notify = new Notify();
        CoffeeMachineService machineService = new CoffeeMachineService(notify, machineAvailable: 3);
        CoffeeMachineController machineController = new CoffeeMachineController(view, machineService, notify);

        machineController.Run();
    }
}