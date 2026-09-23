using System;
using System.Collections.Generic;
using System.Text;

namespace CoffeeShop.Models
{
    public class IngredientRequirement
    {
        public IngredientType Ingredient { get; }
        public int Quantity { get; }

        public IngredientRequirement(IngredientType ingredient, int quantity)
        {
            Ingredient = ingredient;
            Quantity = quantity;
        }
    }
}
