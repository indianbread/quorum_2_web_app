using System.Net;

namespace kata_frameworkless_web_app
{
    public class Response : IResponse
    {
        public WebHeaderCollection Headers { get; set; }
        public string RedirectLocation { get; set; }
        public int StatusCode { get; set; } = (int)HttpStatusCode.OK;
        public string Body { get; set; }
    }
}