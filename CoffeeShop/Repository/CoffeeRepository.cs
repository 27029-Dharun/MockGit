using CoffeeShop.Models;

namespace CoffeeShop.Repository;

internal class CoffeeRepository
{
    private readonly List<Coffee> _coffee;

    public CoffeeRepository()
    {
        _coffee = LoadCoffee();
    }

    private List<Coffee> LoadCoffee()
    {
        List<Coffee> list =
        [
            new Coffee(1, CoffeeMenu.Espresso, TimeSpan.FromSeconds(10),
            [
                new(IngredientType.CoffeeBeans, 10),
                new(IngredientType.Water, 30)
            ]),
            new Coffee(2, CoffeeMenu.Cappuccino, TimeSpan.FromSeconds(15),
            [
                new(IngredientType.CoffeeBeans, 10),
                new(IngredientType.Milk, 100),
                new(IngredientType.Water, 30)
            ]),
            new Coffee(3, CoffeeMenu.Latte, TimeSpan.FromSeconds(12),
            [
                new(IngredientType.CoffeeBeans, 10),
                new(IngredientType.Milk, 150),
                new(IngredientType.Water, 30)
            ]),
            new Coffee(4, CoffeeMenu.Americano, TimeSpan.FromSeconds(12),
            [
                new(IngredientType.CoffeeBeans, 10),
                new(IngredientType.Water, 150),
            ])
        ];

        return list;
    }

    public Coffee GetByName(CoffeeMenu coffeeName)
    {
        return _coffee.FirstOrDefault(coffee => coffee.Name == coffeeName)
            ?? throw new KeyNotFoundException("Coffee not found");
    }
}
