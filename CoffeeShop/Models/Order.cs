namespace CoffeeShop.Models;

internal class Order
{
    public Order(Guid guid, Coffee coffee, int userId)
    {
        Id = guid;
        Coffee = coffee;
        UserId = userId;
    }

    public int UserId { get; set; }

    public Guid Id { get; init; }

    public Coffee Coffee { get; set; }
}
