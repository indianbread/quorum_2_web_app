using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kata_frameworkless_web_app.controllers;
using kata.users.domain;
using kata.users.shared;

namespace kata_frameworkless_web_app
{
    public class RequestRouter
    {
        public RequestRouter(IEnumerable<IController> controllers)
        {
            _controllers.AddRange(controllers);
        }
        
        private readonly List<IController> _controllers = new List<IController>();


        public async Task RouteRequestAsync(IRequest request, IResponse response)
        {
            if (request.Url.Segments.Length == 1)
            {
                var indexController = _controllers.Find(controller => controller.GetType() == typeof(IndexController));
                await indexController.HandleGetRequestAsync(request, response);
            }
            else
            {
                await HandleResourceGroupRequestAsync(request, response);
            }

        }
        
        private async Task HandleResourceGroupRequestAsync(IRequest request, IResponse response)
        {
            try
            {
                var controller = GetController(request);
                await HandleRequestAsync(controller, request, response);

            }
            catch
            {
                response.StatusCode = (int) HttpStatusCode.NotFound;
                await StreamOutput.GenerateResponseBodyAsync(response, "Not found");
            }
        }

        private IController GetController(IRequest request)
        {
            var resourceGroup = request.Url.Segments[1];
            var controllerName = Formatter.FormatControllerName(resourceGroup) + "Controller";
            var controllerType = Type.GetType($"kata_frameworkless_web_app.controllers.{controllerName}", true, true);
            return _controllers.FirstOrDefault(controller => controller.GetType() == controllerType);
        }
        
        private static async Task HandleRequestAsync(IController controller, IRequest request, IResponse response)
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