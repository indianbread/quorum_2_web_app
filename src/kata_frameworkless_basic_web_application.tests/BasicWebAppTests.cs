using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using kata_frameworkless_web_app;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests
{
    public class BasicWebAppTests : IClassFixture<WebAppFixture>
    {
        private WebAppFixture _webAppFixture;
        private readonly HttpClient _client;

        public BasicWebAppTests(WebAppFixture webAppFixture)
        {
            _webAppFixture = webAppFixture;
            _client = new HttpClient();
        }

        [Fact]
        public async Task GET_Index_ReturnsMessageWithNameAndTime()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var response = _client.GetAsync("http://localhost:8080/").GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"the time on the server is {currentDatetime}", responseBody);
 
            response.Dispose();

        }

        [Fact]
        public async Task POST_Names_ReturnsStatus200_IfAddedSuccessfully()
        {
            HttpContent content = new StringContent("Bob", Encoding.UTF8);
        
            var response = _client.PostAsync("http://localhost:8080/add/names/", content).GetAwaiter().GetResult();
            //get awaiter - track the request
            // get result = wait until i get a result
            //guarantees a response, and is blocking because we want it to block until a response as been received before we assert

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response.Dispose();
        }


    }
}
