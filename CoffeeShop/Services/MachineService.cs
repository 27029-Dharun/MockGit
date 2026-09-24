using CoffeeShop.Models;
using CoffeeShop.Repository;

namespace CoffeeShop.Services
{
    internal class MachineService
    {
        private readonly MachineRepository _machineRepository;

        public MachineService(MachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
        }

        public CoffeeMachine? AssignOrder(Order order)
        {
            List<CoffeeMachine> machines = this._machineRepository.GetAllMachine();

            foreach (CoffeeMachine machine in machines)
            {
                if(machine.TryAssign(order))
                {
                    return machine;
                }
            }
            return null;
        }

        public void ReleaseOrder(CoffeeMachine machine)
        {
            machine.Release();

        }
    }
}
