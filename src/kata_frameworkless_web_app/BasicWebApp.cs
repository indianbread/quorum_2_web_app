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
            _userController = new UserController();
            _listener = new HttpListener();
        }
        
        private readonly UserController _userController;
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
            await HandleRequest(request, response);
        }

        private async Task HandleRequest(HttpListenerRequest request, HttpListenerResponse response) //TODO: may be make a new class called index controller
        {
            switch (request.Url.AbsolutePath)
            {
                case "/":
                    await _userController.HandleGetIndexRequest(response);
                    break;
                case "/names":
                    await _userController.HandleRequest(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.Conflict;
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