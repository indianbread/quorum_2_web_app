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
               await _userController.HandleGetIndexRequest(response);
            }
            else
            {
                await HandleResourceGroupRequest(request, response);
            }

        }

        private async Task HandleResourceGroupRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.Segments[1])
            {
                case "users/":
                    await _userController.HandleRequest(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}