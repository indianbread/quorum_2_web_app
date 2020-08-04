using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace kata_frameworkless_web_app
{
    public class UserRepository
    {
        public UserRepository(DbContext usersDatabase)
        {
            _usersDatabase = usersDatabase;
        }
        private readonly DbContext _usersDatabase;
        

        public async Task<List<string>> GetUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<User> FindUserByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}