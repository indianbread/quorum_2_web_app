using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kata.users.shared;



namespace kata_frameworkless_basic_web_application.tests
{
    public class TestUserUserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public TestUserUserRepository()
        {
            _users = new List<User>()
            {
                new User() {FirstName = "Nhan"},
                new User() {FirstName = "Bob"}
            };
            
        }
        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        public Task<IEnumerable<kata.users.shared.User>> GetUsersAsync()
        {
            throw new System.NotImplementedException();
        }

        Task<kata.users.shared.User> IUserRepository.FindUserByNameAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public User FindUserByName(string name)
        {
            return _users.FirstOrDefault(user => user.FirstName == name);
        }

        public async Task AddUserAsync(string name)
        {
             _users.Add(new User() {FirstName = name}); 
        }

        public void RemoveData()
        {
            throw new System.NotImplementedException();
        }
    }
}