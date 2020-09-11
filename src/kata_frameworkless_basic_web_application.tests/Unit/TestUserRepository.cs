using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kata.users.shared;

namespace kata_frameworkless_basic_web_application.tests.Unit
{
    public class TestUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public TestUserRepository()
        {
            _users = new List<User>()
            {
                new User() {Id = "1", FirstName = "Nhan"},
                new User() {Id = "2", FirstName = "Bob"}
            };
            
        }
        
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return _users;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return _users.FirstOrDefault(user => user.FirstName == name);
        }

        public async Task CreateUserAsync(User user)
        {
             _users.Add(user); 
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return _users.FirstOrDefault(user => user.Id == userId);
        }

        public async Task<User> UpdateUser(User userToUpdate)
        {
            var user = await GetUserByIdAsync(userToUpdate.Id);
            user.FirstName = userToUpdate.FirstName;
            return user;
        }

        public void RemoveData()
        {
            throw new System.NotImplementedException();
        }

    }
}