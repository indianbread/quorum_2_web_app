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
            var userRepository = new UserRepository();
            userRepository.RemoveDataFromUsers();
            userRepository.AddUser(AwsSecretManager.GetSecret());
            var userService = new UserService(userRepository);
            var userController = new UserController(userService);
            var basicWebApp = new BasicWebApp(userController);
            basicWebApp.Start();
            basicWebApp.Stop();
        }
        
    }
}