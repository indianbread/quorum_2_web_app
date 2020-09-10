using System;
using System.Threading;
using kata_frameworkless_web_app;
using kata.users.domain;
using kata.users.repositories.DynamoDb;
using kata.users.shared;
using System.Threading.Tasks;

namespace kata_frameworkless_basic_web_application.tests.Integration
{
    public class HttpListenerFixture : IDisposable
    {
        public HttpListenerFixture()
        {
            _userRepository = new DynamoDbUserRepository();
            var userService = new UserService(_userRepository);
            var server = new Server(userService);
            var webAppThread = new Thread(async () => await server.Start());
            webAppThread.Start();
        }

        private readonly IUserRepository _userRepository;

        public void Dispose()
        {
            var userToRestore = new User() { Id = "1", FirstName = "Bob" };
            _userRepository.UpdateUser(userToRestore).GetAwaiter().GetResult();


            //var userToDelete = _userRepository.GetUserByNameAsync("Jane");

            //_userRepository.DeleteUser(userToDelete.Id);

        }
    }
}