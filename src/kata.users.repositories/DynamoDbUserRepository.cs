using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using kata.users.domain;
using kata.users.shared;

namespace kata.users.repositories
{
    public class DynamoDbUserRepository : IUserRepository
    {
        private readonly IDynamoDBContext _dbContext;
        public DynamoDbUserRepository(IDynamoDBContext dbContext) //TODO: where to create the context and the dynamodb client? 
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _dbContext.ScanAsync<User>(conditions).GetRemainingAsync();
        }

        public User FindUserByName(string name)
        {
            //TODO: same validations as service
            //to keep it as a pure function - so if a name is not valid it will always throw an exception
            name = name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1);
            var scanConditions = new List<ScanCondition>() { new ScanCondition("FirstName", ScanOperator.Equal, name)};
            var searchResults = _dbContext.ScanAsync<User>(scanConditions).GetRemainingAsync().GetAwaiter().GetResult();
            return searchResults.FirstOrDefault();
        }

        public async Task AddUserAsync(string name) //TODO: is error checking required?
        {
            var userId = Guid.NewGuid().ToString();
            var newUser = new User() { Id = userId, FirstName = name};
            await _dbContext.SaveAsync(newUser);
        }
        
    }
}