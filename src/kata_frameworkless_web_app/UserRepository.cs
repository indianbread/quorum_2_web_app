using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace kata_frameworkless_web_app
{
    public class UserRepository
    {
        public UserRepository(SqLiteDbContext database)
        {
            _database = database;
        }
        private readonly SqLiteDbContext _database;
        

        public IEnumerable<string> GetUsers()
        {
            return _database.Users.Select(user => user.FirstName);
        }

        public User FindUserByName(string name)
        {
            return _database.Users.FirstOrDefault(users => users.FirstName == name);
        }

        public void AddUser(string name)
        {
            _database.Users.Add(new User() {FirstName = name});
            _database.SaveChanges();
        }
    }
}