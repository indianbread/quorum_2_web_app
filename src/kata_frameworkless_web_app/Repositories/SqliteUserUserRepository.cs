using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app.Repositories
{
    public class SqliteUserUserRepository : IUserRepository
    {
        public SqliteUserUserRepository()
        {
            _context = InitializeDatabase();
        }
        
        private readonly SqLiteDbContext _context;

        public void RemoveData()
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
                Console.WriteLine("Database error: " + e.Message);

            }
            return context;
        }
        
        
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return _context.Users;
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