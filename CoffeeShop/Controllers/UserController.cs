using CoffeeShop.Services;
using CoffeeShop.Views;

namespace CoffeeShop.Controllers
{
    internal class UserController
    {
        private readonly UserService _userService;
        private readonly ConsoleView _view;
        private readonly CoffeeMachineController _coffeeMachineController;

        public UserController(UserService service, ConsoleView view, CoffeeMachineController coffeeMachineController)
        {
            _coffeeMachineController = coffeeMachineController;
            _userService = service;
            _view = view;
        }

        public int LogIn()
        {
            string email = this._view.GetEmail();
            string password = this._view.GetPassword();

            int userId = this._userService.LogIn(email, password);

            return userId;
        }

        internal async Task Authenticate()
        {
            while (true)
            {
                int userId = LogIn();

                if(userId == -1)
                {
                    this._view.PrintWarning("Enter a valid email and password");
                    continue;
                }

                if (userId > 0)
                {
                    this._coffeeMachineController.SetUserId(userId);
                    await this._coffeeMachineController.Run();
                }
            }
        }
    }
}
