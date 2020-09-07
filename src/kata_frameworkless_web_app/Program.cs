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
            var dynamoDbClient = DynamoDb.CreatClient(true);
            var dynamoDbUserRepository = new DynamoDbUserRepository(dynamoDbClient);
            var userService = new UserService(dynamoDbUserRepository);
            var secretUser = AwsSecretManager.GetSecret();
            var addSecretUserRequest = new CreateUserRequest() {FirstName = secretUser};
           // await userService.CreateUser(addSecretUserRequest);
            var userController = new UserController(userService);
            var controllers = new List<IController>() {userController};
            var server = new Server(controllers, userService);
            await server.Start();
        }
        
    }
}