using System.Net;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class RequestHandler
    {
        public RequestHandler(UserController userController)
        {
            _userController = userController;
        }
        private readonly UserController _userController;
        
        public async Task HandleRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.AbsolutePath)
            {
                case "/":
                    await _userController.HandleGetIndexRequest(response);
                    break;
                case "/names":
                    await _userController.HandleRequest(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
        
    }
}