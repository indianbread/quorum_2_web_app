using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using kata.users.domain;
using kata.users.shared;

namespace kata_frameworkless_web_app
{
    public class RequestRouter
    {
        public RequestRouter(UserService userService)
        {
            _userService = userService;
            _controllers = new List<IController>() { new UserController(_userService)};
        }
        
        private readonly List<IController> _controllers;
        private readonly UserService _userService;

        public async Task RouteRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.Segments.Length == 1)
            {
               await GetGreetingAsync(response);
            }
            else
            {
                await HandleResourceGroupRequestAsync(request, response);
            }

        }

        private async Task GetGreetingAsync(HttpListenerResponse response)
        {
            var users = await _userService.GetUsers();
            var names = users.Select(user => user.FirstName).ToList();
            var responseString = Formatter.FormatGreeting(names);
            await Response.GenerateBodyAsync(response, responseString);
        }

        private async Task HandleResourceGroupRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                var controller = GetController(request);
                await HandleRequestAsync(controller, request, response);

            }
            catch
            {
                response.StatusCode = (int) HttpStatusCode.NotFound;
                await Response.GenerateBodyAsync(response, "Not found");
            }
        }

        private IController GetController(HttpListenerRequest request)
        {
            var resourceGroup = request.Url.Segments[1];
            var controllerName = Formatter.FormatControllerName(resourceGroup) + "Controller";
            var controllerType = Type.GetType($"kata_frameworkless_web_app.{controllerName}", true, true);
            return _controllers.FirstOrDefault(controller => controller.GetType() == controllerType);
        }
        
        private static async Task HandleRequestAsync(IController controller, HttpListenerRequest request, HttpListenerResponse response) //TODO: merge 
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    await controller.HandleGetRequestAsync(request, response);
                    break;
                case "POST":
                    await controller.HandlePostRequestAsync(request, response);
                    break;
                case "PUT":
                    await controller.HandlePutRequestAsync(request, response);
                    break;
                case "DELETE":
                    await controller.HandleDeleteRequestAsync(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}