using CoffeeShop.Models;

namespace CoffeeShop.Repository;

internal class Inventory
{
    private readonly List<InventoryItem> _inventory;

    public Inventory()
    {
        _inventory = JsonRepository.Load<InventoryItem>("inventory.json");
    }

    public InventoryItem GetByName(IngredientType type)
    {
        return _inventory.FirstOrDefault(item => item.Ingredient == type) ?? throw new Exception();
    }

    public bool HasIngredient(IngredientType ingredient, int quantity)
    {
        InventoryItem item = GetByName(ingredient);

        lock(_inventory)
        {
            if (item.Quantity < quantity)
            {
                return false;
            }
        }

        return true;
    }

    public void DecrementStock(IngredientType ingredient, int quantity)
    {
        InventoryItem item = GetByName(ingredient);

        lock (_inventory)
        {
            item.Quantity -= quantity;

            JsonRepository.Save("inventory.json", _inventory);
        }
    }

    internal void RefillStocks()
    {
        foreach (InventoryItem ingredient in _inventory)
        {
            lock (_inventory)
            {
                ingredient.Quantity = ingredient.MaximumQuantity;

                JsonRepository.Save("inventory.json", _inventory);
            }
        }
    }
}
