using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace kata_frameworkless_web_app
{
    public class BasicWebApp
    {
        public BasicWebApp()
        {
            _listener = new HttpListener();
            AddPrefixes();
        }
        
        private static readonly List<string> _users = new List<string>() {"Nhan"};

        private void AddPrefixes()
        {
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
            var context = await _listener.GetContextAsync();
            var request = context.Request;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            var response = context.Response;
            ProcessResponse(request, response);
            
        }

        private static void ProcessResponse(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    var responseString = GetIndexResponseString();
                    GenerateResponseBody(response, responseString);
                    break;
                case "POST":
                    AddUser(request, response);
                    break;
                default:
                    response.StatusCode = 404;
                    break;
            }

            response.Close();
        }

        private static void AddUser(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                var name = GetNameFromRequestBody(request);
                if (_users.Contains(name))
                {
                    response.StatusCode = 409;
                    throw new ArgumentException("Error: User already exists");
                }
                _users.Add(name);
                response.StatusCode = 200;
            }
            catch (Exception e)
            {
                GenerateResponseBody(response, e.Message);
            }

        }

        private static string GetNameFromRequestBody(HttpListenerRequest request)
        {
            var body = request.InputStream;
            var reader = new StreamReader(body, Encoding.UTF8);
            var name = reader.ReadToEnd();
            reader.Close();
            return name;
        }

        private static async Task GenerateResponseBody(HttpListenerResponse response, string responseString)
        {
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        private static string GetIndexResponseString()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var usersString = _users.First();
            if (_users.Count <= 1) return "Hello " + usersString + " - the time on the server is " + currentDatetime;;
            for (var i = 1; i < _users.Count - 1; i++)
            {
                usersString += ", " + _users[i];
            }
            usersString += " and " + _users.Last();

            return "Hello " + usersString + " - the time on the server is " + currentDatetime;
        }
        
        public void Stop()
        {
            IsListening = false;
            _listener.Stop();
        }

    }
}