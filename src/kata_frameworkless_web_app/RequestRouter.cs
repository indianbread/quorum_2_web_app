using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class RequestRouter
    {
        public RequestRouter(UserController userController)
        {
            _userController = userController;
        }
        private readonly UserController _userController;
        
        public async Task RouteRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.Segments.Length == 1)
            {
               await _userController.GetGreetingAsync(response);
            }
            else
            {
                await HandleResourceGroupRequestAsync(request, response);
            }

        }
        
        private async Task HandleResourceGroupRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var segments = request.Url.Segments;
            foreach (var segment in segments)
            {
                Console.WriteLine(segment);
            }
            switch (request.Url.Segments[1])
            {
                case "users":
                    await RouteUserRequestAsync(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
        
        private async Task RouteUserRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    await _userController.HandleGetRequestAsync(request, response);
                    break;
                case "POST":
                    await _userController.HandlePostRequestAsync(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}