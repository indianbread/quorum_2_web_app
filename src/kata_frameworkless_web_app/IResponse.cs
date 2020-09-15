using System;
using System.IO;
using System.Net;
using System.Text;

namespace kata_frameworkless_web_app
{
    public interface IResponse
    {
        WebHeaderCollection Headers { get; set; }
        string RedirectLocation { get; set; }
        int StatusCode { get; set; }
        string Body { get; set; }
    }
}