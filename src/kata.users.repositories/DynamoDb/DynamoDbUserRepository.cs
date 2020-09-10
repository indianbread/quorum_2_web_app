using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using kata.users.shared;

namespace kata.users.repositories.DynamoDb
{
    public class DynamoDbUserRepository : IUserRepository
    {
        private Table UserTable
        {
            get
            {
                if (_userTable != null) return _userTable;
                _client = DynamoDb.CreatClient(true);
                _userTable = Table.LoadTable(_client, "NhanUser"); //singleton
                return _userTable;
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var scanFilter = new ScanFilter();
            var results = await UserTable.Scan(scanFilter).GetRemainingAsync();
            return results.Select(ConvertDocumentToUser);
        }
        
        public async Task<User> GetUserByNameAsync(string name)
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("FirstName", ScanOperator.Equal, name);
            var searchResults = await UserTable.Scan(scanFilter).GetRemainingAsync();
            return searchResults.Count == 0 ? null : ConvertDocumentToUser(searchResults.FirstOrDefault());
        }

        public async Task AddUserAsync(string name)
        {
            var userId = Guid.NewGuid().ToString();
            var newUser = new User() { Id = userId, FirstName = name };
            var newUserDocument = ConvertUserToDocument(newUser);
            await UserTable.PutItemAsync(newUserDocument);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("Id", ScanOperator.Equal, userId);
            var searchResults = await UserTable.Scan(scanFilter).GetRemainingAsync();
            return searchResults.Count == 0 ? null : ConvertDocumentToUser(searchResults.FirstOrDefault());
        }

        public async Task UpdateUser(User userToUpdate)
        {
            var user = new Document();
            user["Id"] = userToUpdate.Id;
            user["FirstName"] = userToUpdate.FirstName;

            await UserTable.UpdateItemAsync(user);
        }

        private static User ConvertDocumentToUser(Document document)
        {
            var id = document["Id"].AsString();
            var firstName = document["FirstName"].AsString();
            
            return new User {Id = id, FirstName = firstName};
        }

        private static Document ConvertUserToDocument(User user)
        {
            var userDocument = new Document();
            userDocument["Id"] = user.Id;
            userDocument["FirstName"] = user.FirstName;
            return userDocument;
        }
        
        private IAmazonDynamoDB _client;
        private Table _userTable;
        
    }
    
}