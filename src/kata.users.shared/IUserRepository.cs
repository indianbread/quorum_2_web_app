using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata.users.shared
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        User FindUserByName(string name);

        Task AddUserAsync(string name);
        
    }
}