namespace CoffeeShopTasks.Models;

internal class Coffee
{
    public Coffee(int id, string name, TimeSpan prepTime, TimeSpan sourceTime)
    {
        this.Id = id;
        this.Name = name;
        this.PreparationTime = prepTime;
        this.SourceTime = sourceTime;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public TimeSpan PreparationTime { get; set; }

    public TimeSpan SourceTime { get; set; }
}
