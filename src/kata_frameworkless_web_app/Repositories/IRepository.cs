using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<User>> GetUsers();

        User FindUserByName(string name);

        void AddUser(string name);

    }
}