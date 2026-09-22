using CoffeeShopTasks.Models;
using CoffeeShopTasks.Services;
using CoffeeShopTasks.Views;

namespace CoffeeShopTasks.Controllers;

internal class CoffeeMachineController
{
    private readonly ConsoleView _view;
    private readonly CoffeeMachineService _coffeeMachineService;
    private readonly Notify _notify;

    public CoffeeMachineController(ConsoleView view, CoffeeMachineService machineService, Notify notifier)
    {
        this._view = view;
        this._coffeeMachineService = machineService;
        this._notify = notifier;
        this._notify.DisplayNotification += this._view.PrintSuccess;
    }

    public void Run()
    {
        while (true)
        {
            CoffeeMenu menu = this._view.GetMainMenuOption();
            Coffee createdCoffee;

            switch (menu)
            {
                case CoffeeMenu.Espresso:
                    createdCoffee = new Coffee(1, "Espresso", TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(2));
                    break;

                case CoffeeMenu.Cappuccino:
                    createdCoffee = new Coffee(1, "Espresso", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
                    break;
                case CoffeeMenu.Latte:
                    createdCoffee = new Coffee(1, "Espresso", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
                    break;
                case CoffeeMenu.Americano:
                    createdCoffee = new Coffee(1, "Espresso", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
                    break;
                case CoffeeMenu.Exit:
                    return;
                default:
                    throw new Exception();
            }

            Task task = this._coffeeMachineService.OrderCoffee(createdCoffee);
        }
    }
}
