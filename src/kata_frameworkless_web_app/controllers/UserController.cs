using System;
using System.Net;
using System.Threading.Tasks;
using kata.users.shared;
using Newtonsoft.Json;

namespace kata_frameworkless_web_app.controllers
{
    public class UserController : IController
    {
        public UserController(IService userService)
        {
            _userService = userService;
        }


        public async Task<IResponse> HandleGetRequestAsync(IRequest request)
        {
            switch (request.Url.Segments.Length)
            {
                case 2:
                    return await HandleGetUsersRequestAsync();
                case 3:
                   return await HandleGetUserByIdRequestAsync(request);
                default:
                    int statusCode = (int) HttpStatusCode.NotFound;
                    return new Response { StatusCode = statusCode };
            }
        }

        public async Task<IResponse> HandleCreateRequestAsync(IRequest request)
        {
            var newUserFirstName = request.GetNameFromPayload();
            User newUser;
            try
            {
                newUser = await _userService.CreateUserAsync(newUserFirstName);
                var redirectLocation = $"/users/{newUser.Id}";
                return new Response { Body = "User added successfully", RedirectLocation = redirectLocation };
            }
            catch (Exception e)
            {
                var statusCode = (int) HttpStatusCode.InternalServerError;
                return new Response { Body = e.Message, StatusCode = statusCode };
            }
        }
        
        public async Task<IResponse> HandleUpdateRequestAsync(IRequest request)
        {
            var userId = request.Url.Segments[2];
            var newName = request.GetNameFromPayload();
            var updatedUserObject = new User() {Id = userId, FirstName = newName};
            string responseString;
            int statusCode;
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(updatedUserObject);
                responseString = JsonConvert.SerializeObject(updatedUser);
                statusCode = (int)HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                responseString = e.Message;
                statusCode = responseString.Contains("User does not exist") ? (int)HttpStatusCode.NotFound : (int) HttpStatusCode.InternalServerError;
            }

            return new Response { Body = responseString, StatusCode = statusCode };

        }

        public async Task<IResponse> HandleDeleteRequestAsync(IRequest request)
        {
            var userId = request.Url.Segments[2];
            string responseString;
            int statusCode;
            try
            {
                await _userService.DeleteUserAsync(userId);
                responseString = "User successfully deleted";
                statusCode = (int)HttpStatusCode.OK;
                
            }
            catch (Exception e)
            {
                statusCode = e.Message.Contains("Forbidden") ? (int) HttpStatusCode.Forbidden : (int) HttpStatusCode.NotFound;
                responseString = e.Message;
            }

            return new Response { Body = responseString, StatusCode = statusCode };
        }

        private async Task<IResponse> HandleGetUserByIdRequestAsync(IRequest request)
        {
            var userId = request.Url.Segments[2];
            string responseString;
            int statusCode;
            try
            {
                var user = await _userService.GetUserById(userId);
                responseString = JsonConvert.SerializeObject(user);
                statusCode = (int)HttpStatusCode.OK;
            }

            catch (Exception e)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                responseString = e.Message;
            }

            return new Response { Body = responseString, StatusCode = statusCode };
        }


        private async Task<IResponse> HandleGetUsersRequestAsync()
        {
            var users = await _userService.GetUsers();
            var responseBody = JsonConvert.SerializeObject(users);
            return new Response { Body = responseBody };
        }

        private readonly IService _userService;
    }
}