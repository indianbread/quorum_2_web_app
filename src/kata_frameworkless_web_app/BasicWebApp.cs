using System;
using System.IO;
using System.Net;

namespace kata_frameworkless_web_app
{
    public class BasicWebApp
    {
        private readonly int _port;
        private readonly HttpListener _server;

        public BasicWebApp(int port)
        {
            _port = port;
            _server = new HttpListener();
        }
        
        public void Start()
        {
            //TODO: microsoft docs recommend not to use these top level wildcards
            _server.Prefixes.Add($"http://*:{_port}/");
            _server.Start();
            Console.WriteLine($"Listening on port {_port}");
        }
        
        public void GetResponse()
        {
            while (true)
            {
                
                var context = _server.GetContext(); // provides access to request/response objects
                var request = context.Request;
                Console.WriteLine($"{request.HttpMethod} {request.Url}");
                var response = context.Response;
                var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
                const string user = "Nhan";
                var responseString = $"Hello {user} - the time on the server is {currentDatetime}";
                var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                //ContentLength64 property must be set explicitly before writing to the returned Stream object
                response.OutputStream.Write(buffer, 0, buffer.Length); // forces send of response
                response.OutputStream.Close();
            } ; 
        }
        

        public void Stop()
        {
            _server.Stop();
            _server.Close();
        }

    }
}