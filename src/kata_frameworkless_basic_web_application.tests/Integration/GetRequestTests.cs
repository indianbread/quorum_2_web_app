using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests.Integration
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
        public async Task GET_Names_ReturnsListOfNames()
        {
            var response = await _httpClient.GetAsync("http://localhost:8080/users/names/");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.Contains("Name List", responseBody);
            
            response.Dispose();
        }
        
        [Theory]
        [InlineData("http://localhost:8080/users/names?action=put")]
        [InlineData("http://localhost:8080/notapath")]
        public async Task GET_IncorrectPath_ReturnsStatus404(string url)
        {
            var response = await _httpClient.GetAsync(url);
            
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
            response.Dispose();
        }
        
    }
}
