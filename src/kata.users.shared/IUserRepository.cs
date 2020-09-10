using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata.users.shared
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByNameAsync(string name);

        Task AddUserAsync(string name);

        Task<User> GetUserByIdAsync(string userId);
        Task UpdateUser(User userId);
    }
}