using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using kata_frameworkless_web_app.Repositories;
using kata.users.domain;
using kata.users.repositories;
using kata.users.shared;


namespace kata_frameworkless_web_app
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
            //var config = new AmazonDynamoDBConfig();
            config.RegionEndpoint = RegionEndpoint.APSoutheast2;
            var dynamoDbClient = new AmazonDynamoDBClient(config);
            var dynamoDbUserContext = new DynamoDBContext(dynamoDbClient);
            var dynamoDbUserRepository = new DynamoDbUserRepository(dynamoDbUserContext);
            var userService = new UserService(dynamoDbUserRepository);
            var secretUser = AwsSecretManager.GetSecret();
            var addSecretUserRequest = new CreateUserRequest() {FirstName = secretUser};
            await userService.CreateUser(addSecretUserRequest);
            var userController = new UserController(userService);
            var basicWebApp = new BasicWebApp(userController);
            basicWebApp.Start();
        }
        
    }
}