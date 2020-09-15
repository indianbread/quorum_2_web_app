using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using kata_frameworkless_web_app.controllers;
using kata.users.domain;
using System.Text;

namespace kata_frameworkless_web_app
{
    public class Server
    {
        public Server(UserService userService, IEnumerable<IController> controllers)
        {
            _requestRouter = new RequestRouter(controllers);
            _listener = new HttpListener();
        }

        public bool IsListening { get; private set; }

        private readonly RequestRouter _requestRouter;
        private readonly HttpListener _listener;

        
        public async Task Start()
        {
            AddPrefixes();
            IsListening = true;
            _listener.Start();
            Console.WriteLine($"Listening on port {Port}" );
            while (IsListening)
            {
                await ProcessRequestAsync();
            }
        }

        private void AddPrefixes()
        {
            _listener.Prefixes.Add($"http://*:{Port}/");
        }
        
        public async Task ProcessRequestAsync()
        {
            var context = await _listener.GetContextAsync();
            IRequest request = new Request(context.Request);
            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            using (var httpListenerResponse = context.Response)
            {
                var response = await _requestRouter.RouteRequestAsync(request);
                await SendResponse(httpListenerResponse, response);
            }
        }

        private async Task SendResponse(HttpListenerResponse httpListenerResponse, IResponse response)
        {

            httpListenerResponse.StatusCode = response.StatusCode;
            httpListenerResponse.RedirectLocation = response.RedirectLocation;
            var buffer = Encoding.UTF8.GetBytes(response.Body);
            httpListenerResponse.ContentLength64 = buffer.Length;
            await httpListenerResponse.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            await httpListenerResponse.OutputStream.DisposeAsync();
        }

        private const int Port = 8080;
        
    }
}