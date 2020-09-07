using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kata.users.shared;

namespace kata.users.repositories
{
    public class ListUserRepository : IUserRepository
    {

        private readonly List<User> _users;

        public ListUserRepository()
        {
            _users = new List<User>();
        }

        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new System.NotImplementedException();
        }

        Task<User> IUserRepository.FindUserByNameAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public User FindUserByName(string name)
        {
            return _users.FirstOrDefault(users => users.FirstName == name);
        }

        public async Task AddUserAsync(string name)
        {
            _users.Add(new User() {FirstName = name});
        }

        public void RemoveData()
        {
            _users.Clear();
        }

    }
}