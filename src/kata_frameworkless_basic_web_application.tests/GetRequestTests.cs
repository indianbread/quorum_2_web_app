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
    [Collection("HttpListener collection")]
    public class GetRequestTests
    {
        public GetRequestTests(HttpListenerFixture httpListenerFixture)
        {
            _httpListenerFixture = httpListenerFixture;
            _httpClient = new HttpClient();
        }
        
        private HttpListenerFixture _httpListenerFixture;
        private readonly HttpClient _httpClient;

        [Fact]
        public async Task GET_Index_ReturnsMessageWithNameAndTime()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var response = await _httpClient.GetAsync("http://localhost:8080/");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"the time on the server is {currentDatetime}", responseBody);
 
            response.Dispose();

        }

        [Fact]
        public async Task POST_Name_ReturnsStatus200_IfAddedSuccessfully()
        {
            HttpContent content = new StringContent("Jane", Encoding.UTF8);
            
            var response = await _httpClient.PostAsync("http://localhost:8080/names/add/", content);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response.Dispose();
        }

        [Fact]
        public async Task POST_Name_ReturnsStatus409_IfNameAlreadyExists() 
        { 
            HttpContent content = new StringContent("Bob", Encoding.UTF8);
            var response1 = await _httpClient.PostAsync("http://localhost:8080/names/add/", content);
            response1.Dispose();
            var response2 = await _httpClient.PostAsync("http://localhost:8080/names/add/", content);
            var response2Body = response2.Content.ReadAsStringAsync().Result;
            
            Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
            Assert.Contains("Name already exists", response2Body);

            response2.Dispose();

        }
    }
}
