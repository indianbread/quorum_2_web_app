using System;
using System.Net;

namespace kata_frameworkless_web_app
{
    public interface IListenerContext
    {
        HttpListenerRequest Request { get; }
        HttpListenerResponse Response { get; }
    }
}
