using CoffeeShop.Models;
using CoffeeShop.Models.Enums;
using CoffeeShop.Services;
using CoffeeShop.Views;

namespace CoffeeShop.Controllers;

internal class CoffeeMachineController
{
    private readonly ConsoleView _view;
    private readonly InventoryService _orderService;
    private readonly OrderService _coffeeMachineService;
    private readonly NotificationService _notify;

    public CoffeeMachineController(ConsoleView view, InventoryService machineService, NotificationService notifier, OrderService coffeeMachineService)
    {
        _view = view;
        _orderService = machineService;
        _coffeeMachineService = coffeeMachineService;
        _notify = notifier;
        _notify.DisplayNotification += this.NotifyUser;
    }
    private int currentUserId { get; set; }

    public void Run()
    {
        while (currentUserId > 0)
        {
            try
            {
                CoffeeMenu menu = _view.GetMainMenuOption();

                if (menu == CoffeeMenu.LogOut)
                {
                    currentUserId = -1;
                    continue;
                }

                InitiateOrder(menu, currentUserId);
            }
            catch (Exception ex)
            {
                _view.PrintError(ex.Message);
                _view.PrintError(ex.StackTrace);
            }
        }
    }

    internal void SetUserId(int userId)
    {
        currentUserId = userId;
    }

    private async Task InitiateOrder(CoffeeMenu menu, int userId)
    {
        Order order = _orderService.ProcessInventory(menu, userId);

        await _coffeeMachineService.SubmitOrder(order);
    }

    private void NotifyUser(string message, int userId)
    {
        if (userId == currentUserId)
        {
            this._view.PrintSuccess(message);
        }
    }
}
