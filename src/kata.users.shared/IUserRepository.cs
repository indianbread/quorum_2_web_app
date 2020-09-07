using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata.users.shared
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> FindUserByNameAsync(string name);

        Task AddUserAsync(string name);
        
    }
}