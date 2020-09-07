using System.Collections.Generic;
using System.Threading;
using kata_frameworkless_web_app;
using kata.users.domain;
using kata.users.repositories;
using kata.users.repositories.DynamoDb;


namespace kata_frameworkless_basic_web_application.tests
{
    public class HttpListenerFixture
    {
        public HttpListenerFixture()
        {
            _userRepository = CreateDynamoDbUserRepository();
            _userService = new UserService(_userRepository);
            _userController = new UserController(_userService);
            _controllers = new List<IController>() {_userController};
            _server = new Server(_controllers, _userService);
            _webAppThread =  new Thread(() => _server.Start());
            _webAppThread.Start();
        }
        private readonly Server _server;
        private Thread _webAppThread;
        private readonly UserService _userService;
        private UserController _userController;
        private kata.users.shared.IUserRepository _userRepository;
        private List<IController> _controllers;

        private static DynamoDbUserRepository CreateDynamoDbUserRepository()
        {
            var dynamoDbClient = DynamoDb.CreatClient(true);
            return new DynamoDbUserRepository(dynamoDbClient);
        }
    }
}