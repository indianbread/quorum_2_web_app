using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace kata_frameworkless_web_app.Repositories
{
    public class DynamoDbUserRepository : IUserRepository
    {
        private readonly IDynamoDBContext _dbContext;
        public DynamoDbUserRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _dbContext.ScanAsync<User>(conditions).GetRemainingAsync();
        }

        public async Task<User> FindUserByName(string name)
        {
            name = name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1);
            var scanConditions = new List<ScanCondition>() { new ScanCondition("FirstName", ScanOperator.Equal, name)};
            var searchResults = await _dbContext.ScanAsync<User>(scanConditions).GetRemainingAsync();
            return searchResults.FirstOrDefault();
        }

        public void AddUser(string name)
        {
            var userId = Guid.NewGuid().ToString();
            var newUser = new User() { Id = userId, FirstName = name};
            _dbContext.SaveAsync(newUser);
        }
    }
}