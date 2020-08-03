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
        public UserController(DbContext usersDatabase)
        {
            _usersDatabase = usersDatabase;
        }

        private readonly DbContext _usersDatabase;

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

        private async Task HandleGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.PathAndQuery)
            {
                case "/names?":
                   await GetNameList(response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }

        private async Task GetNameList(HttpListenerResponse response)
        {
            var nameList = ResponseFormatter.GenerateNamesList(_usersDatabase.Names); //TODO: move this to user service
            await ResponseFormatter.GenerateResponseBody(response, nameList);
        }

        private async Task HandlePostRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.QueryString["action"])
            {
                case "add":
                    await _usersDatabase.AddName(request, response); //todo: move this to user service
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }
    }
}