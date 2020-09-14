using System.Net;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app.controllers
{
    public interface IController
    {
        Task HandleGetRequestAsync(IRequest request, IResponse response);

        Task HandlePostRequestAsync(IRequest request, IResponse response);

        Task HandleDeleteRequestAsync(IRequest request, IResponse response);
        Task HandlePutRequestAsync(IRequest request, IResponse response);
    }
}