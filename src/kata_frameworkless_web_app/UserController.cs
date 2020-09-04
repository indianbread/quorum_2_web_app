using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using kata.users.domain;
using kata.users.shared;
using Newtonsoft.Json;
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
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }

        public async Task HandleGetIndexRequestAsync(HttpListenerResponse response)
        {
            var users = await _userService.GetUsers();
            var names = users.Select(user => user.FirstName).ToList();
            var responseString = ResponseFormatter.GetGreeting(names);
            await GenerateResponseBodyAsync(response, responseString);
        }
        
        private async Task HandleGetRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.Segments[2])
            {
                case "names/":
                   await HandleGetUsersRequestAsync(response);
                   break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
            }
        }

        private async Task HandleGetUsersRequestAsync(HttpListenerResponse response)
        {
            var users = await _userService.GetUsers();
            var responseBody = JsonConvert.SerializeObject(users);
            await GenerateResponseBodyAsync(response, responseBody);
        }

        private async Task HandlePostRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
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
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await GenerateResponseBodyAsync(response, e.Message);
            }

        }

        private async Task HandlePostNameRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var newUserFirstName = GetNameFromRequestBody(request);
            var createUserRequest = new CreateUserRequest() {FirstName = newUserFirstName};
            await _userService.CreateUser(createUserRequest);
            response.AppendHeader("Location", $"/users/{newUserFirstName}/");
            await GenerateResponseBodyAsync(response, "User added successfully");
        }

        private static string GetNameFromRequestBody(HttpListenerRequest request)
        {
            var body = request.InputStream;
            using (var reader = new StreamReader(body, Encoding.UTF8))
            {
                var data = reader.ReadToEnd();
                var user = JObject.Parse(data);
                return (user["FirstName"] ?? "").Value<string>();
            }
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