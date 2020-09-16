using System.Threading.Tasks;
using kata_frameworkless_web_app;
using kata_frameworkless_web_app.controllers;

namespace kata_frameworkless_basic_web_application.tests.Unit.TestDoubles
{
    public class MockUserController : IController
    {

        public async Task<IResponse> HandleDeleteRequestAsync(IRequest request)
        {
            return new Response { Body = "MockUserController HandleDeleteRequestAsync called" };
        }

        public async Task<IResponse> HandleGetRequestAsync(IRequest request)
        {


            return new Response { Body = "MockUserController HandleGetRequestAsync called" };

        }

        public async Task<IResponse> HandleCreateRequestAsync(IRequest request)
        {
            return new Response { Body = "MockUserController HandlePostRequestAsync called" };

        }

        public async Task<IResponse> HandleUpdateRequestAsync(IRequest request)
        {
            return new Response { Body = "MockUserController HandlePutRequestAsync called" };
        }
    }
}
