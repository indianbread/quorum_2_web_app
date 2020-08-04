using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace kata_frameworkless_web_app
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContext userDatabase = new SqLiteDbContext();
            var userRepository = new UserRepository(userDatabase);
            var userService = new UserService(userRepository);
            var userController = new UserController(userService);
            var basicWebApp = new BasicWebApp(userController);
            basicWebApp.Start();
            basicWebApp.Stop();
        }
        
    }
}