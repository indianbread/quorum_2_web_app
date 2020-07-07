using System;
using System.IO;
using System.Net;

namespace kata_frameworkless_web_app
{
    public class BasicWebApp
    {
        public void Run()
        {
            var server = new HttpListener();
            const int _port = 8080;
            server.Prefixes.Add($"http://localhost:{_port}/");
            server.Start();
            Console.WriteLine($"Listening on port {_port}");
            while (true)
            {
            var context = server.GetContext(); // provides access to request/response objects
            var request = context.Request;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            var response = context.Response;
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            const string user = "Nhan";
            var responseString = $"Hello {user} - the time on the server is {currentDatetime}";
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            //ContentLength64 property must be set explicitly before writing to the returned Stream object
            response.OutputStream.Write(buffer, 0, buffer.Length);
            
            } ; 
            // forces send of response
            // server.Stop(); // never reached...
            // server.Close();
            // return response;

        }

    }
}