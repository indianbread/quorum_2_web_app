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
        public UserRepository()
        {
            _context = InitializeDatabase();
            RemoveDataFromUsers();
        }
        
        private readonly SqLiteDbContext _context;

        private void RemoveDataFromUsers()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
        }
        
        private static SqLiteDbContext InitializeDatabase()
        {
            var context = new SqLiteDbContext();
            try
            {
                context.Database.EnsureCreated();
            }
            catch (Exception e)
            {
                Console.WriteLine("Database error");

            }
            return context;
        }
        
        
        public IEnumerable<string> GetUsers()
        {
            return _context.Users.Select(user => user.FirstName);
        }

        public User FindUserByName(string name)
        {
            return _context.Users.FirstOrDefault(users => users.FirstName == name);
        }

        public void AddUser(string name)
        {
            _context.Users.Add(new User() {FirstName = name});
            _context.SaveChanges();
        }
    }
}