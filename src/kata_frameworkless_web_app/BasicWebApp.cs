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
        private bool _isListening;
        
        public void Start()
        {
            _isListening = true;
            _listener.Start();
            Console.WriteLine($"Listening on port {Port}" );
            while (_isListening)
            { 
                _listener.BeginGetContext(ProcessRequest, null);
            }
        }

        private async void ProcessRequest(IAsyncResult asyncResult)
        {
            var context = _listener.EndGetContext(asyncResult);
            _listener.BeginGetContext(ProcessRequest, null);
            var request = context.Request;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            var response = context.Response;
            await ProcessResponse(request, response);
        }

        private static async Task ProcessResponse(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    var responseString = GetIndexResponseString();
                    await GenerateResponseBody(response, responseString);
                    break;
                case "POST":
                    switch (request.Url.AbsolutePath)
                    {
                        case "/names/add/":
                            await AddUser(request, response);
                            break;
                        default:
                            response.StatusCode = 404;
                            break;
                    }
                    response.Close();
                    break;
                default:
                    response.StatusCode = 404;
                    break;
            }

            response.Close();
        }

        private static async Task AddUser(HttpListenerRequest request, HttpListenerResponse response)
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
                await GenerateResponseBody(response, e.Message);
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
            _isListening = false;
            _listener.Stop();
        }

    }
}