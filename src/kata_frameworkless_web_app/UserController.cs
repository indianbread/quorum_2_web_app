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
        

        public async Task HandleGetRequestAsync(HttpListenerResponse response)
        {
            var users = await _userService.GetUsers();
            var responseBody = JsonConvert.SerializeObject(users);
            await Response.GenerateBodyAsync(response, responseBody);
        }
        
        public async Task HandlePostRequestAsync(HttpListenerRequest request, HttpListenerResponse response) //TODO: change to put
        {
            try
            {
                var newUserFirstName = Request.GetNameFromPayload(request);
                var createUserRequest = new CreateUserRequest() {FirstName = Formatter.FormatName(newUserFirstName)};
                await _userService.CreateUser(createUserRequest);
                response.AppendHeader("Location", $"/users/{newUserFirstName}");
                await Response.GenerateBodyAsync(response, "User added successfully");
            }
            catch (Exception e)
            {
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await Response.GenerateBodyAsync(response, e.Message);
                
            }

        }
        
    }
}