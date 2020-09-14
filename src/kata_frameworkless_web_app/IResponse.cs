using System;
using System.IO;
using System.Net;
using System.Text;

namespace kata_frameworkless_web_app
{
    public interface IResponse : IDisposable
    {
        Encoding ContentEncoding { get; set; }
        long ContentLength64 { get; set; }
        string ContentType { get; set; }
        WebHeaderCollection Headers { get; set; }
        Stream OutputStream { get; }
        string RedirectLocation { get; set; }
        int StatusCode { get; set; }
        bool SendChunked { get; set; }
        string StatusDescription { get; set; }
        void AppendHeader(string name, string value);
        CookieCollection Cookies { get; set; }
        bool KeepAlive { get; set; }
        Version ProtocolVersion { get; set; }
    }
}