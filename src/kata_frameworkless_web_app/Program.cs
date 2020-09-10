using System.Collections.Generic;
using System.Threading.Tasks;
using kata.users.domain;
using kata.users.repositories;
using kata.users.repositories.DynamoDb;
using kata.users.shared;


namespace kata_frameworkless_web_app
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dynamoDbUserRepository = new DynamoDbUserRepository();
            var userService = new UserService(dynamoDbUserRepository);
          //  var secretUser = AwsSecretManager.GetSecret();
          //  await userService.CreateUser(secretUser);
            
            var server = new Server(userService);
            await server.Start();
        }
        
    }
}