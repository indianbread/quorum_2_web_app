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
            _requestHandler = new RequestHandler(userController);
            _listener = new HttpListener();
        }
        
        private readonly RequestHandler _requestHandler;
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

        private async Task ProcessRequestAsync()
        {
            var context = await _listener.GetContextAsync();
            var request = context.Request;
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            using (var response = context.Response)
            {
                await _requestHandler.HandleRequestAsync(request, response);
            }
        }
        
        public void Stop()
        {
            IsListening = false;
            _listener.Stop();
        }

    }
}