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
        public DynamoDbUserRepository(bool useDynamoDbLocal)
        {
            _useDynamoDbLocal = useDynamoDbLocal;
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

        public async Task<User> GetUserByIdAsync(string userId)
        {
            Primitive partitionKey = new Primitive(userId);
            GetItemOperationConfig config = new GetItemOperationConfig()
            {
                AttributesToGet = new List<string>() { "Id", "FirstName" },
            };

            var userDocument = await UserTable.GetItemAsync(partitionKey, config);
            return userDocument == null ? null : ConvertDocumentToUser(userDocument);

        }

        public async Task CreateUserAsync(User newUser)
        {
            var newUserDocument = ConvertUserToDocument(newUser);
            await UserTable.PutItemAsync(newUserDocument);
        }

        public async Task<User> UpdateUserAsync(User userToUpdate)
        {
            var user = new Document();
            user["Id"] = userToUpdate.Id;
            user["FirstName"] = userToUpdate.FirstName;

            var config = new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
            };

            var result = await UserTable.UpdateItemAsync(user, config);

            return ConvertDocumentToUser(result);
            
        }

        public async Task DeleteUserAsync(User userToDelete)
        {
            await UserTable.DeleteItemAsync(userToDelete.Id);
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

        private IAmazonDynamoDB Client
        {
            get
            {
                if (_client != null) return _client;
                _client = DynamoDb.CreatClient(_useDynamoDbLocal);
                return _client;

            }
        }

        private Table UserTable
        {
            get
            {
                if (_userTable != null) return _userTable;
                _userTable = Table.LoadTable(Client, TableName); //singleton
                return _userTable;
            }
        }

        private IAmazonDynamoDB _client;
        private Table _userTable;
        const string TableName = "NhanUser";
        private readonly bool _useDynamoDbLocal;
    }
    
}