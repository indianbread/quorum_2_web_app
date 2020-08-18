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
        public BasicWebApp(UserController userController)
        {
            _userController = userController;
            _listener = new HttpListener();
        }
        
        private readonly UserController _userController;
        private readonly HttpListener _listener;
        private bool IsListening;
        private const int Port = 8080;

        private void AddPrefixes()
        {
            _listener.Prefixes.Add($"http://*:{Port}/");
        }
        
        public void Start()
        {
            AddPrefixes();
            IsListening = true;
            _listener.Start();
            Console.WriteLine($"Listening on port {Port}" );
            while (IsListening)
            {
                ProcessRequestAsync().GetAwaiter().GetResult();
            }
        }

        private async Task ProcessRequestAsync() //TODO: log the result
        {
            var context = await _listener.GetContextAsync();
            var request = context.Request;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            var response = context.Response;
            await HandleRequestAsync(request, response);
        }

        private async Task HandleRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
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
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
            response.Close();
        }
        
        public void Stop()
        {
            IsListening = false;
            _listener.Stop();
        }

    }
}