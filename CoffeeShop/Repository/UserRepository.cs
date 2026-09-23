using System.Collections.Concurrent;
using CoffeeShop.Models;

namespace CoffeeShop.Repository
{
    internal class UserRepository
    {
        private readonly ConcurrentBag<User> _users =
        [
            new User()
            {
                Id = 1,
                Name = "Dharun",
                Email = "dharun@gmail.com",
                Password = "12345678",
            }
        ];

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public User? GetByEmail(string email)
        {
            return _users.FirstOrDefault(x => x.Email == email);
        } 
    }
}
