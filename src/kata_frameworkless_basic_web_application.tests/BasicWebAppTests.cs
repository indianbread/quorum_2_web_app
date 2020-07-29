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
    public class BasicWebAppTests : IClassFixture<WebAppFixture>, IClassFixture<HttpClientFixture>
    {
        private WebAppFixture _webAppFixture;
        private readonly HttpClientFixture _httpClientFixture;
        
        public BasicWebAppTests(WebAppFixture webAppFixture, HttpClientFixture httpClientFixture)
        {
            _webAppFixture = webAppFixture;
            _httpClientFixture = httpClientFixture;
        }

        [Fact]
        public async Task GET_Index_ReturnsMessageWithNameAndTime()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var response = _httpClientFixture.Client.GetAsync("http://localhost:8080/").GetAwaiter().GetResult();
            //var response = _httpClientFixture.GetAsync("http://localhost:8080/").GetAwaiter().GetResult();
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"the time on the server is {currentDatetime}", responseBody);
 
            response.Dispose();

        }

        [Fact]
        public async Task POST_Name_ReturnsStatus200_IfAddedSuccessfully()
        {
            HttpContent content = new StringContent("Bob", Encoding.UTF8);
            
            var response = _httpClientFixture.Client.PostAsync("http://localhost:8080/add/names/", content).GetAwaiter().GetResult();
            //var response = _httpClientFixture.PostAsync("http://localhost:8080/add/names/", content).GetAwaiter().GetResult();
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response.Dispose();
        }

        [Fact]
        public async Task POST_Name_ReturnsStatus409_IfNameAlreadyExists()
        {
            HttpContent content = new StringContent("Bob", Encoding.UTF8);
            var response1 = _httpClientFixture.Client.PostAsync("http://localhost:8080/add/names/", content).GetAwaiter().GetResult();
            //var response1 = _httpClientFixture.PostAsync("http://localhost:8080/add/names/", content).GetAwaiter().GetResult();
            response1.Dispose();
            var response2 = _httpClientFixture.Client.PostAsync("http://localhost:8080/add/names/", content).GetAwaiter().GetResult();
            //var response2 = _httpClientFixture.PostAsync("http://localhost:8080/add/names/", content).GetAwaiter().GetResult();
            var response2Body = response2.Content.ReadAsStringAsync().Result;
            
            Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
            Assert.Contains("User already exists", response2Body);

            response2.Dispose();

        }


    }
}
