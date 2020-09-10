using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kata.users.repositories.Sqlite;
using kata.users.shared;

namespace kata.users.repositories
{
    public class SqLiteUserUserRepository : IUserRepository
    {
        public SqLiteUserUserRepository()
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
        
        
        public IEnumerable<User> GetUsers()
        {
            return _context.Users;
        }

        public User FindUserByName(string name)
        {
            return _context.Users.FirstOrDefault(users => users.FirstName == name);
        }

        public Task<IEnumerable<User>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task AddUserAsync(string name)
        {
            await _context.Users.AddAsync(new User() {FirstName = name});
            await _context.SaveChangesAsync();
        }

        public Task<User> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUser(User userId)
        {
            throw new NotImplementedException();
        }
    }
}

