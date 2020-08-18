using System.Collections.Generic;
using System.Linq;

namespace kata_frameworkless_web_app
{
    public class ListRepository : IRepository
    {

        private readonly List<User> _users;

        public ListRepository()
        {
            _users = new List<User>();
        }

        public IEnumerable<string> GetUsers()
        {
            return _users.Select(user => user.FirstName);
        }

        public User FindUserByName(string name)
        {
            return _users.FirstOrDefault(users => users.FirstName == name);
        }

        public void AddUser(string name)
        {
            _users.Add(new User() {FirstName = name});
        }

        public void RemoveData()
        {
            _users.Clear();
        }

    }
}