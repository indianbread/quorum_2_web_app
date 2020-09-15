using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kata_frameworkless_web_app.controllers;
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


        public async Task<IResponse> RouteRequestAsync(IRequest request)
        {
            if (request.Url.Segments.Length == 1)
            {
                var indexController = _controllers.Find(controller => controller.GetType() == typeof(IndexController));
                return await indexController.HandleGetRequestAsync(request);
            }
            else
            {
                return await HandleResourceGroupRequestAsync(request);
            }

        }
        
        private async Task<IResponse> HandleResourceGroupRequestAsync(IRequest request)
        {
            try
            {
                var controller = GetController(request);
                return await HandleRequestAsync(controller, request);

            }
            catch
            {
                var statusCode = (int) HttpStatusCode.NotFound;
                return new Response { StatusCode = statusCode, Body = "Not Found" };
            }
        }

        private IController GetController(IRequest request)
        {
            var resourceGroup = request.Url.Segments[1];
            var controllerName = Formatter.FormatControllerName(resourceGroup) + "Controller";
            var controllerType = Type.GetType($"kata_frameworkless_web_app.controllers.{controllerName}", true, true);
            return _controllers.FirstOrDefault(controller => controller.GetType() == controllerType);
        }
        
        private static async Task<IResponse> HandleRequestAsync(IController controller, IRequest request)
        {
            return request.HttpMethod switch
            {
                "GET" => await controller.HandleGetRequestAsync(request),
                "POST" => await controller.HandlePostRequestAsync(request),
                "PUT" => await controller.HandlePutRequestAsync(request),
                "DELETE" => await controller.HandleDeleteRequestAsync(request),
                _ => new Response { StatusCode = (int)HttpStatusCode.NotFound },
            };
        }
    }
}