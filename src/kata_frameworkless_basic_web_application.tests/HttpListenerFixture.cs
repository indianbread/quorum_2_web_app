using System;
using System.Threading;
using kata_frameworkless_web_app;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace kata_frameworkless_basic_web_application.tests
{
    public class HttpListenerFixture
    {
        public HttpListenerFixture()
        {
            _userRepository = new UserRepository();
            _userRepository.RemoveData();
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
        private IRepository _userRepository;

    }
}