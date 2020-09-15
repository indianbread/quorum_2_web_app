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
        
        public async Task<IResponse> HandleGetRequestAsync(IRequest request)
        {
            var users = await _userService.GetUsers();
            var names = users.Select(user => user.FirstName).ToList();
            var responseString = Formatter.FormatGreeting(names);
            return new Response { Body = responseString };
        }

        Task<IResponse> IController.HandlePostRequestAsync(IRequest request)
        {
            throw new System.NotImplementedException();
        }

        Task<IResponse> IController.HandleDeleteRequestAsync(IRequest request)
        {
            throw new System.NotImplementedException();
        }

        Task<IResponse> IController.HandlePutRequestAsync(IRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}