using System.Threading.Tasks;
using kata_frameworkless_web_app;
using kata_frameworkless_web_app.controllers;

namespace kata_frameworkless_basic_web_application.tests.Unit.TestDoubles
{
    public class MockIndexController : IController
    {

        public async Task<IResponse> HandleDeleteRequestAsync(IRequest request)
        {

            return new Response { Body = "MockIndexController HandleDeleteRequestAsync called" };
        }

        public async Task<IResponse> HandleGetRequestAsync(IRequest request)
        {
            return new Response { Body = "MockIndexController HandleGetRequestAsync called" };

        }

        public async Task<IResponse> HandlePostRequestAsync(IRequest request)
        {
            return new Response { Body = "MockIndexController HandlePostRequestAsync called" };

        }

        public async Task<IResponse> HandlePutRequestAsync(IRequest request)
        {
            return new Response { Body = "MockIndexController HandlePutRequestAsync called" };
        }
    }
}
