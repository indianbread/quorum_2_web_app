using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using kata_frameworkless_web_app.controllers;
using kata.users.domain;



namespace kata_frameworkless_web_app
{
    public class Server
    {
        public Server(UserService userService, IEnumerable<IController> controllers)
        {
            _requestRouter = new RequestRouter(controllers);
            _listener = new HttpListener();
        }
        
        private readonly RequestRouter _requestRouter;
        private readonly HttpListener _listener;

        
        public async Task Start()
        {
            AddPrefixes();
            _isListening = true;
            _listener.Start();
            Console.WriteLine($"Listening on port {Port}" );
            while (_isListening)
            {
               await ProcessRequestAsync();
            }
        }

        private void AddPrefixes()
        {
            _listener.Prefixes.Add($"http://*:{Port}/");
        }
        
        private async Task ProcessRequestAsync()
        {
            var context = await _listener.GetContextAsync();
            IRequest request = new Request(context.Request);
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            using (IResponse response = new Response(context.Response))
            {
                await _requestRouter.RouteRequestAsync(request, response);
            }
        }
        
        private bool _isListening;
        private const int Port = 8080;
        
    }
}