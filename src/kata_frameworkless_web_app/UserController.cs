using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kata.users.domain;
using kata.users.shared;
using Newtonsoft.Json;

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
            var responseString = Formatter.FormatGreeting(names);
            await Response.GenerateBodyAsync(response, responseString);
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
            await Response.GenerateBodyAsync(response, responseBody);
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
                await Response.GenerateBodyAsync(response, e.Message);
            }

        }

        private async Task HandlePostNameRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var newUserFirstName = Request.GetNameFromPayload(request);
            var createUserRequest = new CreateUserRequest() {FirstName = Formatter.FormatName(newUserFirstName)};
            await _userService.CreateUser(createUserRequest);
            response.AppendHeader("Location", $"/users/{newUserFirstName}/");
            await Response.GenerateBodyAsync(response, "User added successfully");
        }

    }
}