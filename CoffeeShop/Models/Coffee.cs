namespace CoffeeShop.Models;

internal class Coffee
{
    public Coffee(int id, CoffeeMenu name, TimeSpan prepTime, List<IngredientRequirement> requirement)
    {
        Id = id;
        Name = name;
        PreparationTime = prepTime;
        Ingredients = requirement;
    }

    public int Id { get; set; }

    public CoffeeMenu Name { get; set; }

    public TimeSpan PreparationTime { get; set; }

    public List<IngredientRequirement> Ingredients { get; set; }
}
