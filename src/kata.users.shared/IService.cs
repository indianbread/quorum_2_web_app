using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata.users.shared
{
    public interface IService
    {
        Task<User> CreateUserAsync(string firstName);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(string Id);
        Task<User> UpdateUserAsync(User newUserDetails);
        Task DeleteUserAsync(string userId);
        void SetSecretUser(string name);

    }
}
