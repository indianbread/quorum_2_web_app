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
            Console.WriteLine("starting app");
            var userRepository = new UserRepository();
           // var userRepository = new ListRepository();
            var userService = new UserService(userRepository);
            //var secretUser = AwsSecretManager.GetSecret();
           // userService.AddSecretUserName(secretUser);
            var userController = new UserController(userService);
            var basicWebApp = new BasicWebApp(userController);
            basicWebApp.Start();
            while (basicWebApp.IsListening)
            {
                basicWebApp.ProcessRequest();
            }
            // basicWebApp.Stop();
        }
        
    }
}