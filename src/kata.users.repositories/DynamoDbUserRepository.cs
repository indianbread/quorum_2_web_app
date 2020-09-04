using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using kata.users.domain;
using kata.users.shared;

namespace kata.users.repositories
{
    public class DynamoDbUserRepository : IUserRepository
    {
        public DynamoDbUserRepository(AmazonDynamoDBClient client)
        {
            _client = client;
            _userTable = Table.LoadTable(_client, "NhanUser");
        }
        private readonly AmazonDynamoDBClient _client;
        private readonly Table _userTable;
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var scanFilter = new ScanFilter();
            var results = await _userTable.Scan(scanFilter).GetRemainingAsync();
            return results.Select(ConvertDocumentToUser);
        }
        
        public async Task<User> FindUserByName(string name)
        {
            var scanFilter = new ScanFilter();
            scanFilter.AddCondition("FirstName", ScanOperator.Equal, name);
            var searchResults = await _userTable.Scan(scanFilter).GetRemainingAsync();
            return searchResults.Count == 0 ? null : ConvertDocumentToUser(searchResults.FirstOrDefault());
        }

        public async Task AddUserAsync(string name)
        {
            var userId = Guid.NewGuid().ToString();
            var newUser = new User() { Id = userId, FirstName = name};
            var newUserDocument = ConvertUserToDocument(newUser);
            await _userTable.PutItemAsync(newUserDocument);
        }
        
        private static User ConvertDocumentToUser(Document document)
        {
            var id = document["Id"].AsString();
            var firstName = document["FirstName"].AsString();
            
            return new User() {Id = id, FirstName = firstName};
        }

        private static Document ConvertUserToDocument(User user)
        {
            var userDocument = new Document();
            userDocument["Id"] = user.Id;
            userDocument["FirstName"] = user.FirstName;
            return userDocument;
        }
        
    }
}