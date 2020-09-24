using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using kata_frameworkless_web_app.controllers;
using System.Text;
using kata.users.shared;

namespace kata_frameworkless_web_app
{
    public class Server
    {
        public Server(IService userService, List<IController> controllers)
        {
            _requestRouter = new RequestRouter(userService, controllers);
            _listener = new HttpListener();
        }

        public bool IsListening { get; private set; }
        private readonly RequestRouter _requestRouter;
        private readonly HttpListener _listener;

        public void Start()
        {
            AddPrefixes();
            IsListening = true;
            _listener.Start();
            Console.WriteLine($"Listening on port {Port}");
        }

        public void ProcessRequest()
        {
            var requests = new List<Task>();
            while (IsListening)
            {
                var context = _listener.GetContext();
                requests.Add(Task.Run(() => ProcessResponseAsync(context)));
                if (requests.Count % 5 == 0)
                {
                    RemoveCompletedTasks(requests);
                }
            }
        }

        private static void RemoveCompletedTasks(List<Task> requests)
        {
            var completedTasks = requests.FindAll(task => task.IsCompleted);
            foreach (var task in completedTasks)
            {
                requests.Remove(task);
                task.Dispose();
            }
        }

        private void AddPrefixes()
        {
            _listener.Prefixes.Add($"http://*:{Port}/");
        }

        private async Task ProcessResponseAsync(HttpListenerContext context)
        {
            IRequest request = new Request(context.Request);
            Console.WriteLine($"{request.HttpMethod} {request.Url}");
            var response = await _requestRouter.RouteRequestAsync(request);
            using (var httpListenerResponse = context.Response)
            {
                SendResponse(httpListenerResponse, response);
            }
        }

        private void SendResponse(HttpListenerResponse httpListenerResponse, IResponse response)
        {

            httpListenerResponse.StatusCode = response.StatusCode;
            httpListenerResponse.RedirectLocation = response.RedirectLocation;
            var buffer = Encoding.UTF8.GetBytes(response.Body);
            httpListenerResponse.ContentLength64 = buffer.Length;
            httpListenerResponse.OutputStream.Write(buffer, 0, buffer.Length);
            httpListenerResponse.OutputStream.Dispose();
        }

        private const int Port = 8080;
        
    }
}