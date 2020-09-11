using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata.users.shared
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByNameAsync(string name);

        Task CreateUserAsync(User user);

        Task<User> GetUserByIdAsync(string userId);
        Task<User> UpdateUser(User userId);
    }
}