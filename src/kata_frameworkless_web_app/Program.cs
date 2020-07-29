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
            var basicWebApp = new BasicWebApp();
            basicWebApp.Start();
            basicWebApp.Stop();
            
        }
        
    }
}