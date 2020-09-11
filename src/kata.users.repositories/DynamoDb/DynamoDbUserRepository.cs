using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
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
                _userTable = Table.LoadTable(_client, TableName); //singleton
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

        public async Task CreateUserAsync(User newUser)
        {
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

        public async Task<User> UpdateUser(User userToUpdate)
        {
            var user = new Document();
            user["Id"] = userToUpdate.Id;
            user["FirstName"] = userToUpdate.FirstName;

            var hashKey = new Primitive(userToUpdate.Id);

            var expression = new Expression();
            expression.ExpressionStatement = "SET Id = :Id";
            expression.ExpressionAttributeValues[":Id"] = userToUpdate.Id;


            var config = new UpdateItemOperationConfig
            {
               // ConditionalExpression = expression,
                ReturnValues = ReturnValues.AllNewAttributes
            };

            var result = await UserTable.UpdateItemAsync(user, config);

            //var attributes = result.GetAttributeNames();
            //foreach (var attribute in attributes)
            //{
            //    Console.WriteLine(attribute);

            //}

            return new User() { Id = result["Id"], FirstName = result["FirstName"] } ;
            
            //var key = new Dictionary<string, AttributeValue>
            //{
            //    { "Id", new AttributeValue(userToUpdate.Id) }
            //};
            //var attributeToUpdate = new Dictionary<string, AttributeValueUpdate>
            //{
            //    { "FirstName", new AttributeValueUpdate(new AttributeValue(userToUpdate.FirstName),AttributeAction.PUT) }
            //};

            //var response = await _client.UpdateItemAsync(TableName, key, attributeToUpdate);
            //var id = response.Attributes["Id"].S;
            //var updatedFirstName = response.Attributes["FirstName"].S;
            //return new User() { Id = id, FirstName = updatedFirstName };
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
        const string TableName = "NhanUser";
        
    }
    
}