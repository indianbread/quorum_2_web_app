using System;
using System.Threading;
using kata_frameworkless_web_app;
using kata.users.domain;
using kata.users.repositories.DynamoDb;
using kata.users.shared;
using System.Threading.Tasks;

namespace kata_frameworkless_basic_web_application.tests.Integration
{
    public class HttpListenerFixture
    {
        public HttpListenerFixture()
        {
            UserRepository = new DynamoDbUserRepository();
            var userService = new UserService(UserRepository);
            var server = new Server(userService);
            var webAppThread = new Thread(async () => await server.Start());
            webAppThread.Start();
        }

        public readonly IUserRepository UserRepository;

    }
}