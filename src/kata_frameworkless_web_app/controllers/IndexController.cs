using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kata.users.domain;
using kata.users.shared;

namespace kata_frameworkless_web_app.controllers
{
    public class IndexController : IController
    {
        public IndexController(UserService userService)
        {
            _userService = userService;
        }
        
        private readonly UserService _userService;
        
        public async Task HandleGetRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            var users = await _userService.GetUsers();
            var names = users.Select(user => user.FirstName).ToList();
            var responseString = Formatter.FormatGreeting(names);
            await StreamOutput.GenerateResponseBodyAsync(response, responseString);
        }

        public Task HandlePostRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            throw new System.NotImplementedException();
        }

        public Task HandleDeleteRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            throw new System.NotImplementedException();
        }

        public Task HandlePutRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}