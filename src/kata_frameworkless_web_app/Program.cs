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
            //wrap db in try-catch
            //ensure error message is user friendly
            //move lines 17 and 18 to repository
            var context = new SqLiteDbContext();
            context.Database.EnsureCreated(); //need this to ensure db is created before its used to prevent errors
            var userRepository = new UserRepository(context);
            var userService = new UserService(userRepository);
            var userController = new UserController(userService);
            var basicWebApp = new BasicWebApp(userController);
            basicWebApp.Start();
            basicWebApp.Stop();
        }
        
    }
}