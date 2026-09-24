namespace CoffeeShop.Models
{
    internal class CoffeeMachine
    {
        public CoffeeMachine(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        public bool IsBusy { get; set; }

        public Order? Order { get; set; }

        public bool TryAssign(Order order)
        {
            if(IsBusy)
            {
                return false;
            }

            IsBusy = true;
            Order = order;
            return true;
        }

        public void Release()
        {
            IsBusy = false;
            Order = null;
        }
    }
}
