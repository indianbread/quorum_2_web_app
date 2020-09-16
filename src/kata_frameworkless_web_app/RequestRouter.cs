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
        public RequestRouter(IService userService, IList<IController> controllers)
        {
            _controllers = controllers;
        }
        
        private IList<IController> _controllers;


        public async Task<IResponse> RouteRequestAsync(IRequest request)
        {
            if (request.Url.Segments.Length == 1)
            {
                var indexController = _controllers.FirstOrDefault(controller => controller.GetType().Name.Contains("IndexController"));
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
            var controllerName = Formatter.FormatControllerName(resourceGroup);
            return _controllers.FirstOrDefault(controller => controller.GetType().Name.Contains(controllerName));
        }
        
        private static async Task<IResponse> HandleRequestAsync(IController controller, IRequest request)
        {
            return request.HttpMethod switch
            {
                "GET" => await controller.HandleGetRequestAsync(request),
                "POST" => await controller.HandleCreateRequestAsync(request),
                "PUT" => await controller.HandleUpdateRequestAsync(request),
                "DELETE" => await controller.HandleDeleteRequestAsync(request),
                _ => new Response { StatusCode = (int)HttpStatusCode.NotFound },
            };
        }
    }
}