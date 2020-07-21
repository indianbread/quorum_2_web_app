using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class BasicWebApp
    {
        public BasicWebApp()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add($"http://*:{Port}/");
        }
        private const int Port = 8080;
        private readonly HttpListener _listener;
        public bool IsListening;
        
        public void Start()
        {
            IsListening = true;
            _listener.Start();
            Console.WriteLine($"Listening on port {Port}" );
        }

        public async void ProcessRequest()
        {
            var context = await _listener.GetContextAsync(); // provides access to request/response objects
            var request = context.Request;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            var response = context.Response;
            var responseString = GetResponseString();
            var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            //ContentLength64 property must be set explicitly before writing to the returned Stream object
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length); // forces send of response
            response.OutputStream.Close();
            response.Close();
        }

        private static string GetResponseString()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            const string user = "Nhan";
            var responseString = $"<HTML><BODY>Hello {user} - the time on the server is {currentDatetime}</BODY></HTML>";
            return responseString;
        }
        
        public void Stop()
        {
            IsListening = false;
            _listener.Stop();
        }

    }
}