using System.Timers;
using CoffeeShop.Models;
using CoffeeShop.Repository;

namespace CoffeeShop.Services;

/// <summary>
/// Coffee machine service.
/// </summary>
internal class InventoryService
{
    private readonly CoffeeRepository _coffeeRepository;
    private readonly Inventory _inventory;
    private readonly NotificationService _notificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryService"/> class.
    /// </summary>
    /// <param name="notifier">notifier</param>
    /// <param name="machineAvailable">Machine count</param>
    public InventoryService(CoffeeRepository repository, Inventory inventory, NotificationService notificationService)
    {
        _coffeeRepository = repository;
        _inventory = inventory;
        _notificationService = notificationService;
        RefillInventory();
    }

    internal Order ProcessInventory(CoffeeMenu menu, int userId)
    {
        Coffee coffee = _coffeeRepository.GetByName(menu);

        if (!this.HasRequiredIngredients(coffee))
        {
            throw new Exception("Insufficient stock");
        }

        this.DecrementIngredients(coffee);
        return new(Guid.NewGuid(), coffee, userId);
    }

    public bool DecrementIngredients(Coffee coffee)
    {
        foreach (var item in coffee.Ingredients)
        {
            this._inventory.DecrementStock(item.Ingredient, item.Quantity);
        }

        return true;
    }

    public bool HasRequiredIngredients(Coffee coffee)
    {
        foreach (var item in coffee.Ingredients)
        {
            bool ingredient = this._inventory.HasIngredient(item.Ingredient, item.Quantity);
            if (!ingredient)
            {
                return false;
            }
        }

        return true;
    }

    internal void RefillInventory()
    {
        System.Timers.Timer timer = new(TimeSpan.FromSeconds(100));
        timer.AutoReset = true;
        timer.Elapsed += Restock;

        timer.Start();
    }

    private void Restock(object? sender, ElapsedEventArgs e)
    {
        this._inventory.RefillStocks();
        this._notificationService.Execute("[Restocking] Completed", 1);
    }
}
