using CoffeeShop.Models;

namespace CoffeeShop.Repository;

internal class MachineRepository
{
    private readonly List<CoffeeMachine> _machines = new();

    public MachineRepository()
    {
        _machines.Add(new CoffeeMachine(1));
        _machines.Add(new CoffeeMachine(2));
        _machines.Add(new CoffeeMachine(3));
    }

    public List<CoffeeMachine> GetAllMachine()
    {
        return _machines;
    }
}
