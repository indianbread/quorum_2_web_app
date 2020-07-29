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
        public BasicWebApp(UserList userList)
        {
            _userList = userList;
            _listener = new HttpListener();

        }
        
        private readonly UserList _userList;
        private const int Port = 8080;
        private readonly HttpListener _listener;
        private bool _isListening;

        private void AddPrefixes()
        {
            _listener.Prefixes.Add($"http://*:{Port}/");
        }
        
        public void Start()
        {
            AddPrefixes();
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

        private async Task ProcessResponse(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    var responseString = ResponseFormatter.GetGreeting(_userList.Names);
                    await GenerateResponseBody(response, responseString);
                    break;
                case "POST":
                    switch (request.Url.AbsolutePath)
                    {
                        case "/names/add/":
                            await AddName(request, response);
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

        private async Task AddName(HttpListenerRequest request, HttpListenerResponse response)
        { 
            var addUserTaskResult = _userList.AddUser(request, response);
            if (!addUserTaskResult.IsCompletedSuccessfully)
            {
                GenerateResponseBody(response, addUserTaskResult.Exception.Message);
            }
        }
        
        private async Task GenerateResponseBody(HttpListenerResponse response, string responseString)
        {
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
        
        public void Stop()
        {
            _isListening = false;
            _listener.Stop();
        }

    }
}