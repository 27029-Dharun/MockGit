using CoffeeShopApp.Enums;
using CoffeeShopApp.Models;

namespace CoffeeShopApp.Services;

/// <summary>
/// Provides the static coffee shop menu.
/// </summary>
public sealed class MenuService
{
    private readonly IReadOnlyList<Coffee> _menu =
    [
        new Coffee
        {
            Type = CoffeeType.Espresso,
            Name = "Espresso",
            Price = 80m,
            SourcingSeconds = 2,
            PreparationSeconds = 4
        },
        new Coffee
        {
            Type = CoffeeType.Americano,
            Name = "Americano",
            Price = 100m,
            SourcingSeconds = 2,
            PreparationSeconds = 5
        },
        new Coffee
        {
            Type = CoffeeType.Cappuccino,
            Name = "Cappuccino",
            Price = 130m,
            SourcingSeconds = 3,
            PreparationSeconds = 6
        },
        new Coffee
        {
            Type = CoffeeType.Latte,
            Name = "Latte",
            Price = 140m,
            SourcingSeconds = 3,
            PreparationSeconds = 7
        },
        new Coffee
        {
            Type = CoffeeType.Mocha,
            Name = "Mocha",
            Price = 150m,
            SourcingSeconds = 3,
            PreparationSeconds = 8
        },
        new Coffee
        {
            Type = CoffeeType.Tea,
            Name = "Tea",
            Price = 60m,
            SourcingSeconds = 1,
            PreparationSeconds = 4
        }
    ];

    /// <summary>
    /// Gets all menu items.
    /// </summary>
    public IReadOnlyList<Coffee> GetAll() => _menu;

    /// <summary>
    /// Finds a menu item by type.
    /// </summary>
    /// <param name="type">Drink type.</param>
    /// <returns>Matching drink or null.</returns>
    public Coffee? GetByType(CoffeeType type) =>
        _menu.FirstOrDefault(coffee => coffee.Type == type);
}
