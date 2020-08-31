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
        
        public async Task HandleRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            if (request.Url.Segments.Length == 1)
            {
               await _userController.HandleGetIndexRequestAsync(response);
            }
            else
            {
                await HandleResourceGroupRequestAsync(request, response);
            }

        }

        private async Task HandleResourceGroupRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.Segments[1])
            {
                case "users/":
                    await _userController.HandleRequestAsync(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}