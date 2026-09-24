namespace CoffeeShop.Models
{
    internal class CoffeeMachine
    {
        public int Id { get; set; }

        public bool IsBusy { get; set; }

        public Order? Order { get; set; }

        public bool TryAssign(Order order)
        {
            if(Order is null)
            {
                return false;
            }

            Order = order;
            return true;
        }

        public void Release()
        {
            Order = null;
        }
    }
}
