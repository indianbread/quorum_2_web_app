using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        User FindUserByName(string name);

        Task AddUserAsync(string name);

    }
}