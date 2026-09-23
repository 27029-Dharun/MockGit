using System.Collections.Concurrent;
using CoffeeShop.Models;

namespace CoffeeShop.Repository
{
    internal class Inventory
    {
        private readonly ConcurrentBag<InventoryItem> _inventory =
        [
            new InventoryItem(
                IngredientType.CoffeeBeans,
                0,
                1000),

            new InventoryItem(
                IngredientType.Water,
                5000,
                10000),

            new InventoryItem(
                IngredientType.Milk,
                2000,
                5000),

            new InventoryItem(
                IngredientType.Sugar,
                1000,
                2000)
        ];

        public InventoryItem GetByName(IngredientType type)
        {
            return _inventory.FirstOrDefault(item => item.Ingredient == type) ?? throw new Exception();
        }

        public InventoryItem? HasIngredient(IngredientType ingredient, int quantity)
        {
            InventoryItem item = GetByName(ingredient);
            if (item.Quantity < quantity)
            {
                return item;
            }

            return null;
        }

        public void DecrementStock(IngredientType ingredient, int quantity)
        {
            InventoryItem item = GetByName(ingredient);

            lock(_inventory)
            {
                item.Quantity -= quantity;
            }
        }

        internal void RefillStocks()
        {
            foreach (InventoryItem ingredient in _inventory)
            {
                lock(_inventory)
                {
                    ingredient.Quantity = ingredient.MaximumQuantity;
                }
            }
        }
    }
}
