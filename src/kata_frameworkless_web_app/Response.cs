using System;
using System.IO;
using System.Net;
using System.Text;

namespace kata_frameworkless_web_app
{
    public class Response : IResponse
    {
        private readonly HttpListenerResponse _httpListenerResponse;

        public Response(HttpListenerResponse httpListenerResponse)
        {
            _httpListenerResponse = httpListenerResponse;
            ContentEncoding = httpListenerResponse.ContentEncoding;
            ContentLength64 = httpListenerResponse.ContentLength64;
            ContentType = httpListenerResponse.ContentType;
            Headers = httpListenerResponse.Headers;
            OutputStream = httpListenerResponse.OutputStream;
            RedirectLocation = httpListenerResponse.RedirectLocation;
            StatusCode = httpListenerResponse.StatusCode;
            SendChunked = httpListenerResponse.SendChunked;
            StatusDescription = httpListenerResponse.StatusDescription;
            Cookies = httpListenerResponse.Cookies;
            KeepAlive = httpListenerResponse.KeepAlive;
            ProtocolVersion = httpListenerResponse.ProtocolVersion;
        }
        public Encoding ContentEncoding { get; set; }
        public long ContentLength64 { get; set; }
        public string ContentType { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public Stream OutputStream { get; }
        public string RedirectLocation { get; set; }
        public int StatusCode { get; set; }
        public bool SendChunked { get; set; }
        public string StatusDescription { get; set; }
        public CookieCollection Cookies { get; set; }
        public bool KeepAlive { get; set; }
        public Version ProtocolVersion { get; set; }
        
        public void AppendHeader(string name, string value)
        {
            _httpListenerResponse.AppendHeader(name, value);
        }
        
        public void Dispose()
        {
            _httpListenerResponse.Close();
        }
    }
}