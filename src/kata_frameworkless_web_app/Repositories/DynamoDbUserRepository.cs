using System.Collections.Generic;
using System.Threading.Tasks;
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

        public User FindUserByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public void AddUser(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}