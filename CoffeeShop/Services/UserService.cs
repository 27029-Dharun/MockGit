using CoffeeShop.Models;
using CoffeeShop.Repository;

namespace CoffeeShop.Services
{
    internal class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository repository) { 
            _userRepository = repository;
        }

        public int LogIn(string email, string password)
        {
            User? user = this._userRepository.GetByEmail(email);

            if (user == null)
            {
                return -1;
            }

            if(user.Password == password)
            {
                return user.Id;
            }

            return -1;
        }
    }
}
