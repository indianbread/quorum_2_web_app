using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using kata_frameworkless_web_app.AwsDynamoDb;
using kata_frameworkless_web_app.Repositories;
using kata_frameworkless_web_app.Services;
using Microsoft.EntityFrameworkCore;

namespace kata_frameworkless_web_app
{
    class Program
    {
        static void Main(string[] args)
        {

            var dynamoDb = new AwsDynamoDb.AwsDynamoDb();
            dynamoDb.CreateClient(true);
            var dynamoDbClient = dynamoDb.Client;
            //var dbClient = new AmazonDynamoDBClient();
            var dynamoDbUserContext = new DynamoDBContext(dynamoDbClient);
            var dynamoDbUserRepository = new DynamoDbUserRepository(dynamoDbUserContext);
            var table = new AwsDynamoDbTable(dynamoDbClient);
            table.CreateTableAsync(UserTableConstant.TableName, UserTableConstant.AttributeDefinitions,
                UserTableConstant.KeySchemaElements, UserTableConstant.ProvisionedThroughput);
            Console.WriteLine("starting app");
            //var userRepository = new SqliteUserUserRepository();
            var userService = new UserService(dynamoDbUserRepository);
            //userRepository.RemoveData();
            var secretUser = AwsSecretManager.GetSecret();
            userService.AddUser(secretUser);
            var userController = new UserController(userService);
            var basicWebApp = new BasicWebApp(userController);
            basicWebApp.Start();
        }
        
    }
}