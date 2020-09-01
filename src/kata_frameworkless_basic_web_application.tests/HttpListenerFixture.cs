using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using kata_frameworkless_web_app;
using kata_frameworkless_web_app.Repositories;
using kata_frameworkless_web_app.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace kata_frameworkless_basic_web_application.tests
{
    public class HttpListenerFixture
    {
        public HttpListenerFixture()
        {
            _userRepository = CreateDynamoDbUserRepository();
            _userService = new UserService(_userRepository);
            _userController = new UserController(_userService);
            _basicWebApp = new BasicWebApp(_userController);
            _webAppThread = new Thread(_basicWebApp.Start);
            _webAppThread.Start();
        }
        private readonly BasicWebApp _basicWebApp;
        private Thread _webAppThread;
        private readonly UserService _userService;
        private UserController _userController;
        private IUserRepository _userRepository;

        public IEnumerable<string> GetNameList()
        {
            return _userService.GetNameList();
        }

        private static IUserRepository CreateDynamoDbUserRepository()
        {
            var config = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
            var dynamoDbClient = new AmazonDynamoDBClient(config);
            var dynamoDbUserContext = new DynamoDBContext(dynamoDbClient);
            return new DynamoDbUserRepository(dynamoDbUserContext);
        }
    }
}