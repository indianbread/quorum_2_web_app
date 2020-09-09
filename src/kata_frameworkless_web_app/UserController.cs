using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kata.users.domain;
using kata.users.shared;
using Newtonsoft.Json;

namespace kata_frameworkless_web_app
{
    public class UserController : IController
    {
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        private readonly UserService _userService;

        public async Task HandleGetRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            switch (request.Url.AbsolutePath)
            {
                case "/users":
                    await HandleGetUsersRequestAsync(response);
                    break;
                case "/users/1":
                    await HandleGetUserByIdRequestAsync(request, response);
                    break;
                default:
                    response.StatusCode = (int) HttpStatusCode.NotFound;
                    await Response.GenerateBodyAsync(response, "Not found");
                    break;
            }
        }

        private async Task HandleGetUserByIdRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var userId = request.Url.Segments[2];
            var user = await _userService.GetUserById(userId);
            var userString = JsonConvert.SerializeObject(user);
            await Response.GenerateBodyAsync(response, userString);
        }


        public async Task HandleGetUsersRequestAsync(HttpListenerResponse response)
        {
            var users = await _userService.GetUsers();
            var responseBody = JsonConvert.SerializeObject(users);
            await Response.GenerateBodyAsync(response, responseBody);
        }
        
        public async Task HandlePostRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var newUserFirstName = Request.GetNameFromPayload(request);
            try
            {
                await _userService.CreateUser(newUserFirstName);
                response.AppendHeader("Location", $"/users/{newUserFirstName}");
                await Response.GenerateBodyAsync(response, "User added successfully");
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await Response.GenerateBodyAsync(response, e.Message);
                
            }

        }

        public Task HandleDeleteRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            throw new NotImplementedException();
        }
    }
}