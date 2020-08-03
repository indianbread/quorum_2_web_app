using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace kata_frameworkless_web_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var nameList = new NameList();
            var nameController = new NameController(nameList);
            var basicWebApp = new BasicWebApp(nameController, nameList);
            basicWebApp.Start();
            basicWebApp.Stop();
            
        }
        
    }
}