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
            var dynamoDbUserRepository = new DynamoDbUserRepository(false);
            var userService = new UserService(dynamoDbUserRepository);
            await AddSecretUser(userService);

            var controllers = new List<IController>()
            {
                new IndexController(userService),
                new UserController(userService)
            };

            var server = new Server(userService, controllers);
            await server.Start();
        }

        private static async Task AddSecretUser(UserService userService)
        {
            try
            {
                var secretUserName = AwsSecretManager.GetSecret();
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