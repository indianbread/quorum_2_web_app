using System;
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
            switch (request.Url.Segments.Length)
            {
                case 2:
                    await HandleGetUsersRequestAsync(response);
                    break;
                case 3:
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
            string responseString;
            try
            {
                var user = await _userService.GetUserById(userId);
                responseString = JsonConvert.SerializeObject(user);
            }

            catch (Exception e)
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
                responseString = e.Message;
            }

            await Response.GenerateBodyAsync(response, responseString);
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
            User newUser;
            try
            {
                newUser = await _userService.CreateUserAsync(newUserFirstName);
                response.RedirectLocation = $"/users/{newUser.Id}";
                await Response.GenerateBodyAsync(response, "User added successfully");
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await Response.GenerateBodyAsync(response, e.Message);
            }
        }
        
        public async Task HandlePutRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var userId = request.Url.Segments[2];
            var newName = Request.GetNameFromPayload(request);
            var updatedUserObject = new User() {Id = userId, FirstName = newName};
            string responseString;
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(updatedUserObject);
                responseString = JsonConvert.SerializeObject(updatedUser);
            }
            catch (Exception e)
            {

                responseString = e.Message;
                response.StatusCode = responseString.Contains("User does not exist") ? (int)HttpStatusCode.NotFound : (int) HttpStatusCode.InternalServerError;
            }

            await Response.GenerateBodyAsync(response, responseString);

        }

        public async Task HandleDeleteRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var userId = request.Url.Segments[2];
            string responseString;
            try
            {
                await _userService.DeleteUserAsync(userId);
                responseString = "User successfully deleted";

            }
            catch (Exception e)
            {
                response.StatusCode = e.Message.Contains("Not Authorized") ? (int) HttpStatusCode.Unauthorized : (int) HttpStatusCode.NotFound;
                responseString = e.Message;
            }

            await Response.GenerateBodyAsync(response, responseString);
        }

    }
}