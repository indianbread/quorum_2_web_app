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
            IsAuthenticated = httpListenerRequest.IsAuthenticated;
            RawUrl = httpListenerRequest.RawUrl;
            IsLocal = httpListenerRequest.IsLocal;
            IsSecureConnection = httpListenerRequest.IsSecureConnection;
            IsWebSocketRequest = httpListenerRequest.IsWebSocketRequest;
            LocalEndPoint = httpListenerRequest.LocalEndPoint;
            ProtocolVersion = httpListenerRequest.ProtocolVersion;
            RemoteEndPoint = httpListenerRequest.RemoteEndPoint;
            RequestTraceIdentifier = httpListenerRequest.RequestTraceIdentifier;
            ServiceName = httpListenerRequest.ServiceName;
            TransportContext = httpListenerRequest.TransportContext;
            UrlReferrer = httpListenerRequest.UrlReferrer;
            UserAgent = httpListenerRequest.UserAgent;
            UserHostAddress = httpListenerRequest.UserHostAddress;
            UserHostName = httpListenerRequest.UserHostName;
            UserLanguages = httpListenerRequest.UserLanguages;
        }
        
        public Uri Url { get; }
        public string HttpMethod { get; }
        public Stream InputStream { get; }
        public string ContentType { get; }
        public long ContentLength64 { get; }
        public NameValueCollection Headers { get; }
        public bool KeepAlive { get; }
        public NameValueCollection QueryString { get; }
        public bool IsAuthenticated { get; }
        public string RawUrl { get; }
        public bool IsLocal { get; }
        public bool IsSecureConnection { get; }
        public bool IsWebSocketRequest { get; }
        public IPEndPoint LocalEndPoint { get; }
        public Version ProtocolVersion { get; }
        public IPEndPoint RemoteEndPoint { get; }
        public Guid RequestTraceIdentifier { get; }
        public string ServiceName { get; }
        public TransportContext TransportContext { get; }
        public Uri UrlReferrer { get; }
        public string UserAgent { get; }
        public string UserHostAddress { get; }
        public string UserHostName { get; }
        public string[] UserLanguages { get; }
    }
}