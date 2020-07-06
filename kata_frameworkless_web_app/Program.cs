using System;
using System.Net;
using System.Xml;

namespace kata_frameworkless_web_app
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("Not supported");
                return;
            }
            
            var server = new HttpListener();
            const int _port = 8080;
            server.Prefixes.Add($"http://localhost:{_port}/");
            server.Start();
            Console.WriteLine($"Listening on port {_port}");
           // while (true)
           // {
                var context = server.GetContext();  // Gets the request
                Console.WriteLine($"{context.Request.HttpMethod} {context.Request.Url}");
                var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
                var buffer = System.Text.Encoding.UTF8.GetBytes($"Hello Nhan - the time on the server is {currentDatetime}");
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);  // forces send of response
                //
           // }
            context.Response.OutputStream.Close();
            server.Stop();  // never reached...

        }
    }
}