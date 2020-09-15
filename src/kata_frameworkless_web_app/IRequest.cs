using System;
using System.Collections.Specialized;
using System.IO;

namespace kata_frameworkless_web_app
{
    public interface IRequest
    {
        Uri Url { get; set; }
        string HttpMethod { get; set; }
        Stream InputStream { get; }
        string ContentType { get; }
        long ContentLength64 { get; }
        NameValueCollection Headers { get; }
        bool KeepAlive { get; }
        NameValueCollection QueryString { get; }

    }
}