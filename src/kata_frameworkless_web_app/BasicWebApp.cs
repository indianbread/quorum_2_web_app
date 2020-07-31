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
        public BasicWebApp(NameList nameList)
        {
            _nameList = nameList;
            _listener = new HttpListener();

        }
        
        private readonly NameList _nameList;

        private readonly HttpListener _listener;
        private bool _isListening;
        private const int Port = 8080;

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
                case "GET": //TODO: make a method called handle get
                    var responseString = ResponseFormatter.GetGreeting(_nameList.Names);
                    await ResponseFormatter.GenerateResponseBody(response, responseString);
                    break;
                case "POST": //TODO: make a method called handle post response
                    switch (request.Url.AbsolutePath)
                    {
                        case "/names/add/":
                            await _nameList.AddName(request, response);
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
        
        public void Stop()
        {
            _isListening = false;
            _listener.Stop();
        }

    }
}