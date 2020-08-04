using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace kata_frameworkless_web_app
{
    public class UserController
    {
        public UserController()
        {
            _userService = new UserService(TODO);
        }

        private readonly UserService _userService;

        public async Task HandleRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    await HandleGetRequest(request, response);
                    break;
                case "POST":
                    await HandlePostRequest(request, response);
                    break;
                default:
                    response.StatusCode = 404;
                    break;
            }
        }

        public async Task HandleGetIndexRequest(HttpListenerResponse response)
        {
            var responseString = ResponseFormatter.GetGreeting(TODO users);
            await ResponseFormatter.GenerateResponseBody(response, responseString);
        }
        
        

        private async Task HandleGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.PathAndQuery)
            {
                case "/names?":
                   await _userService.GetNameList(response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
        
        private async Task HandlePostRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.QueryString["action"])
            {
                case "add":
                    await _userService.AddName(request, response); //todo: move this to user service
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}