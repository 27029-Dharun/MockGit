namespace CoffeeShop.Models
{
    internal class InventoryItem
    {
        public InventoryItem(IngredientType coffeeBeans, int quantity, int maxQuantity)
        {
            Ingredient = coffeeBeans;
            Quantity = quantity;
            MaximumQuantity = maxQuantity;
        }

        public IngredientType Ingredient { get; }

        public int Quantity { get; set; }

        public int MaximumQuantity { get; }
    }
}