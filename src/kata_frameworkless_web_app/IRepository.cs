using System.Collections.Generic;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public interface IRepository
    {
        IEnumerable<string> GetUsers();

        User FindUserByName(string name);

        void AddUser(string name);
        void RemoveData();
    }
}