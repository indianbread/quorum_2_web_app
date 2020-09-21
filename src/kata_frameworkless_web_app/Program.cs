using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using kata_frameworkless_web_app.controllers;
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
            var useDynamoDbLocal = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_ENV"));
            var dynamoDbUserRepository = new DynamoDbUserRepository(useDynamoDbLocal);
            var userService = new UserService(dynamoDbUserRepository);
            
            await AddSecretUser(userService);

            var controllers = new List<IController>()
            {
                new IndexController(userService),
                new UserController(userService)
            };

            var server = new Server(userService, controllers);
            server.Start();
            await server.ProcessRequestAsync();

        }

        private static async Task AddSecretUser(UserService userService)
        {
            try
            {
                var secretUserName = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SECRET_NAME")) ? 
                    Environment.GetEnvironmentVariable("SECRET_USER") : AwsSecretManager.GetSecret();
                userService.SetSecretUser(secretUserName);
                await userService.CreateUserAsync(secretUserName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}