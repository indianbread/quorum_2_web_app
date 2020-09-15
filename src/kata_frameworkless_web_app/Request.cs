using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

namespace kata_frameworkless_web_app
{
    public class Request : IRequest
    {
        public Request(HttpListenerRequest httpListenerRequest)
        {
            Url = httpListenerRequest.Url;
            HttpMethod = httpListenerRequest.HttpMethod;
            InputStream = httpListenerRequest.InputStream;
            ContentType = httpListenerRequest.ContentType;
            ContentLength64 = httpListenerRequest.ContentLength64;
            Headers = httpListenerRequest.Headers;
            KeepAlive = httpListenerRequest.KeepAlive;
            QueryString = httpListenerRequest.QueryString;
        }
        
        public Uri Url { get; set; }
        public string HttpMethod { get; set; }
        public Stream InputStream { get; }
        public string ContentType { get; }
        public long ContentLength64 { get; }
        public NameValueCollection Headers { get; }
        public bool KeepAlive { get; }
        public NameValueCollection QueryString { get; }
    }
}