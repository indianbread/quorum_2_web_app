using System.Collections.Generic;

namespace kata_frameworkless_web_app.Services
{
    public interface IUserService
    {
        void AddSecretUserName(string name);
        
        IEnumerable<string> GetNameList();
        
        RequestResult AddUser(string name);

    }
}