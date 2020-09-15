using System;
using System.Net;

namespace kata_frameworkless_web_app
{
    public class ListenerContext : IListenerContext
    {
        public ListenerContext(HttpListenerContext httpListenerContext)
        {
            Request = httpListenerContext.Request;
            Response = httpListenerContext.Response;
        }

        public HttpListenerRequest Request { get; }

        public HttpListenerResponse Response { get; }
    }
}
