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
        public UserController(UserService userService)
        {
            _userService = userService;
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
            var names = _userService.GetNameList();
            var responseString = ResponseFormatter.GetGreeting(names.ToList());
            await GenerateResponseBody(response, responseString);
        }
        
        private async Task HandleGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.PathAndQuery)
            {
                case "/names?":
                   await HandleGetNameListRequest(response);
                   break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }

        private async Task HandleGetNameListRequest(HttpListenerResponse response)
        {
            var names = _userService.GetNameList();
            var nameListFormatted = ResponseFormatter.GenerateNamesListBody(names);
            await GenerateResponseBody(response, nameListFormatted);
        }

        private async Task HandlePostRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.QueryString["action"])
            {
                case "add":
                    await HandlePostNameRequest(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }

        private async Task HandlePostNameRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            var newUserName = GetNameFromRequestBody(request);
            var result = await _userService.AddName(newUserName, response);
            await GenerateResponseBody(response, result);
        }

        private static string GetNameFromRequestBody(HttpListenerRequest request)
        {
            var body = request.InputStream;
            var reader = new StreamReader(body, Encoding.UTF8);
            var name = reader.ReadToEnd();
            reader.Close();
            return name;
        }
        
        public static async Task GenerateResponseBody(HttpListenerResponse response, string responseString)
        {
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}