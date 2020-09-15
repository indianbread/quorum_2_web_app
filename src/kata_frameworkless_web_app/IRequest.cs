using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;

namespace kata_frameworkless_web_app
{
    public interface IRequest
    {
        Uri Url { get; }
        string HttpMethod { get; }
        Stream InputStream { get; }
        string ContentType { get; }
        long ContentLength64 { get; }
        NameValueCollection Headers { get; }
        bool KeepAlive { get; }
        NameValueCollection QueryString { get; }

    }
}