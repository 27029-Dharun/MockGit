using CoffeeShop.Models;

namespace CoffeeShop.Repository;

internal class Inventory
{
    private readonly List<InventoryItem> _inventory;
    private readonly object _lock = new();

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
        lock (_lock)
        {
            InventoryItem item = GetByName(ingredient);
            return item.Quantity >= quantity;
        }
    }

    public void DecrementStock(IngredientType ingredient, int quantity)
    {
        lock (_lock)
        {
            InventoryItem item = GetByName(ingredient);

            if (item.Quantity < quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for {ingredient}. Requested: {quantity}, Available: {item.Quantity}.");
            }

            item.Quantity -= quantity;
            JsonRepository.Save("inventory.json", _inventory);
        }
    }

    internal void RefillStocks()
    {
        lock (_lock)
        {
            foreach (InventoryItem ingredient in _inventory)
            {
                ingredient.Quantity = ingredient.MaximumQuantity;
            }

            JsonRepository.Save("inventory.json", _inventory);
        }
    }
}
