using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
            _userUserRepository = new TestUserUserRepository();
            _userService = new UserService(_userUserRepository);
            _userController = new UserController(_userService);
            _basicWebApp = new BasicWebApp(_userController);
            _webAppThread = new Thread(_basicWebApp.Start);
            _webAppThread.Start();
        }
        private readonly BasicWebApp _basicWebApp;
        private Thread _webAppThread;
        private readonly UserService _userService;
        private UserController _userController;
        private IUserRepository _userUserRepository;

        public IEnumerable<string> GetNameList()
        {
            return _userService.GetNameList();
        }

    }
}