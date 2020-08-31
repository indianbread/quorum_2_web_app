using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
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
            // var dynamoDbClient = new AwsDynamoDb.AwsDynamoDb().Client;
            // var table = new AwsDynamoDbTable(dynamoDbClient);
            // table.CreateTableAsync("Users");
            Console.WriteLine("starting app");
            var userRepository = new SqliteUserUserRepository();
            var userService = new UserService(userRepository);
            userRepository.RemoveData();
            var secretUser = AwsSecretManager.GetSecret();
            userService.AddSecretUserName(secretUser);
            var userController = new UserController(userService);
            var basicWebApp = new BasicWebApp(userController);
            basicWebApp.Start();
        }
        
    }
}