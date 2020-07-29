using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace kata_frameworkless_basic_web_application.tests
{
    public class HttpClientFixture : IDisposable
    {
        public HttpClientFixture()
        {
            Client = new HttpClient();
        }

        public HttpClient Client { get; }


        public void Dispose()
        {
            Client.Dispose();
        }
    }
}