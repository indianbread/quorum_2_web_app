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
        
        public async Task HandleGetRequestAsync(IRequest request, IResponse response)
        {
            var users = await _userService.GetUsers();
            var names = users.Select(user => user.FirstName).ToList();
            var responseString = Formatter.FormatGreeting(names);
            await StreamOutput.GenerateResponseBodyAsync(response, responseString);
        }

        public Task HandlePostRequestAsync(IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }

        public Task HandleDeleteRequestAsync(IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }

        public Task HandlePutRequestAsync(IRequest request, IResponse response)
        {
            throw new System.NotImplementedException();
        }
    }
}