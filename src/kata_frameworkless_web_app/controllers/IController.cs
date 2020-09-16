using System.Net;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app.controllers
{
    public interface IController
    {
        Task<IResponse> HandleGetRequestAsync(IRequest request);

        Task<IResponse> HandleCreateRequestAsync(IRequest request);

        Task<IResponse> HandleDeleteRequestAsync(IRequest request);

        Task<IResponse> HandleUpdateRequestAsync(IRequest request);
    }
}