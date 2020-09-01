using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using kata_frameworkless_web_app.Services;
using Newtonsoft.Json.Linq;

namespace kata_frameworkless_web_app
{
    public class UserController
    {
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        private readonly UserService _userService;

        public async Task HandleRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.HttpMethod)
            {
                case "GET":
                    await HandleGetRequestAsync(request, response);
                    break;
                case "POST":
                    await HandlePostRequestAsync(request, response);
                    break;
                default:
                    response.StatusCode = 404;
                    break;
            }
        }

        public async Task HandleGetIndexRequestAsync(HttpListenerResponse response)
        {
            var names = _userService.GetNameList();
            var responseString = ResponseFormatter.GetGreeting(names.ToList());
            await GenerateResponseBodyAsync(response, responseString);
        }
        
        private async Task HandleGetRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.Segments[2])
            {
                case "names/":
                   await HandleGetNameListRequestAsync(response);
                   break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }

        private async Task HandleGetNameListRequestAsync(HttpListenerResponse response)
        {
            var names = _userService.GetNameList();
            var nameListFormatted = ResponseFormatter.GenerateNamesListBody(names);
            await GenerateResponseBodyAsync(response, nameListFormatted);
        }

        private async Task HandlePostRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.Segments[2])
            {
                case "add/":
                    await HandlePostNameRequestAsync(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }

        private async Task HandlePostNameRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var newUserName = GetNameFromRequestBody(request);
            var result = _userService.AddUser(newUserName);
            response.StatusCode = (int) result.StatusCode;
            var responseMessage = result.IsSuccess ? result.SuccessMessage : result.ErrorMessage;
            response.AppendHeader("Location", $"/users/{newUserName}/");
            await GenerateResponseBodyAsync(response, responseMessage);
        }

        private static string GetNameFromRequestBody(HttpListenerRequest request)
        {
            var body = request.InputStream;
            var reader = new StreamReader(body, Encoding.UTF8);
            var data = reader.ReadToEnd();
            reader.Close();
            var user = JObject.Parse(data);
            return (user["FirstName"] ?? "").Value<string>();
        }

        private static async Task GenerateResponseBodyAsync(HttpListenerResponse response, string responseString)
        {
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            await response.OutputStream.DisposeAsync();
        }
    }
}