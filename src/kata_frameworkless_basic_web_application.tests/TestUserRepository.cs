using System.Collections.Generic;
using System.Linq;
using Castle.Core.Resource;
using kata_frameworkless_web_app;

namespace kata_frameworkless_basic_web_application.tests
{
    public class TestUserRepository : IRepository
    {
        private readonly List<User> _users;

        public TestUserRepository()
        {
            _users = new List<User>()
            {
                new User() {FirstName = "Nhan"},
                new User() {FirstName = "Bob"}
            };
            
        }
        public IEnumerable<string> GetUsers()
        {
            return _users.Select(user => user.FirstName);
        }

        public User FindUserByName(string name)
        {
            return _users.FirstOrDefault(user => user.FirstName == name);
        }

        public void AddUser(string name)
        {
            _users.Add(new User() {FirstName = name}); 
        }

        public void RemoveData()
        {
            throw new System.NotImplementedException();
        }
    }
}