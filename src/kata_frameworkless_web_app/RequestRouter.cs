using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using kata.users.domain;

namespace kata_frameworkless_web_app
{
    public class RequestRouter
    {
        public RequestRouter(List<IController> controllers, UserService userService)
        {
            _controllers = controllers;
            _userService = userService;
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
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.NotFound;
                await Response.GenerateBodyAsync(response, e.Message);
            }
        }

        private IController GetController(HttpListenerRequest request)
        {
            Console.WriteLine(_controllers.First().GetType());
            var resourceGroup = request.Url.Segments[1];
            var controllerName = Formatter.FormatControllerName(resourceGroup) + "Controller";
            //var controllerType = typeof(IController).Assembly.GetType(controllerName, true, true);
            //var controllerType = Type.GetType(controllerName, true, false);
            //Console.WriteLine(controllerType);
           // return _controllers.FirstOrDefault(controller => controller.GetType() == controllerType);
            return _controllers.FirstOrDefault();
        }
        
        private static async Task HandleRequestAsync(IController controller, HttpListenerRequest request, HttpListenerResponse response) //TODO: merge 
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    await controller.HandleGetRequestAsync(response);
                    break;
                case "POST":
                    await controller.HandlePostRequestAsync(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}