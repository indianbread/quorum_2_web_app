using System.Net;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public interface IController
    {
        Task HandleGetRequestAsync(HttpListenerRequest request, HttpListenerResponse response);

        Task HandlePostRequestAsync(HttpListenerRequest request, HttpListenerResponse response);

        Task HandleDeleteRequestAsync(HttpListenerRequest request, HttpListenerResponse response);
        Task HandlePutRequestAsync(HttpListenerRequest request, HttpListenerResponse response);
    }
}