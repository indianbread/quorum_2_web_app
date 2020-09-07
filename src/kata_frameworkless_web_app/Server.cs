using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using kata.users.domain;


namespace kata_frameworkless_web_app
{
    public class Server
    {
        public Server(List<IController> controllers, UserService userService)
        {
            _requestRouter = new RequestRouter(controllers, userService); //TODO: how/where to pass in these dependencies?
            _listener = new HttpListener();
        }
        
        private readonly RequestRouter _requestRouter;
        private readonly HttpListener _listener;
        private bool _isListening;
        private const int Port = 8080;
        
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
            var request = context.Request;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            using (var response = context.Response)
            {
                await _requestRouter.RouteRequestAsync(request, response);
            }
        }
        
    }
}