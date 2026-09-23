namespace CoffeeShop.Models;

internal class InventoryItem
{
    public InventoryItem(IngredientType ingredient, int quantity, int maximumQuantity)
    {
        Ingredient = ingredient;
        Quantity = quantity;
        MaximumQuantity = maximumQuantity;
    }

    public IngredientType Ingredient { get; set; }

    public int Quantity { get; set; }

    public int MaximumQuantity { get; set; }
}