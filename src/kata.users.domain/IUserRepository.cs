using System.Collections.Generic;
using System.Threading.Tasks;
using kata.users.shared;

namespace kata.users.domain
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        User FindUserByName(string name);

        Task AddUserAsync(string name);
        
    }
}