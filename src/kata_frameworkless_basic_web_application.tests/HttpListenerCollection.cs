using System.Net;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests
{
    [CollectionDefinition("HttpListener collection")]
    public class HttpListenerCollection : ICollectionFixture<HttpListenerFixture>
    {
        
    }
}