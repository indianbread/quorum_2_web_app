using System;
using System.Collections.Generic;
using System.Threading;
using kata_frameworkless_web_app;
using kata.users.domain;
using kata.users.repositories.DynamoDb;
using kata.users.shared;
using System.Threading.Tasks;
using kata_frameworkless_web_app.controllers;

namespace kata_frameworkless_basic_web_application.tests.Integration
{
    public class HttpListenerFixture
    {
        public HttpListenerFixture()
        {
            UserRepository = new DynamoDbUserRepository(true);
            _userService = new UserService(UserRepository);
            SetUpSecretUser().GetAwaiter().GetResult();
            var controllers = SetUpControllers();
            var server = new Server(_userService);
            var webAppThread = new Thread(async () => await server.Start());
            webAppThread.Start();
        }

        private async Task SetUpSecretUser()
        {
            _userService.SetSecretUser("Nhan");
            try
            {
                await _userService.CreateUserAsync("Nhan");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }

        private List<IController> SetUpControllers()
        {
            return new List<IController>()
            {
                new IndexController(_userService),
                new UserController(_userService),
            };
        }

        public readonly IUserRepository UserRepository;
        private readonly IService _userService;
    }
}