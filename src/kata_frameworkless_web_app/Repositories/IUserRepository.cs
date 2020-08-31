using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();

        User FindUserByName(string name);

        void AddUser(string name);

    }
}