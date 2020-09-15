using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using kata.users.shared;
using Newtonsoft.Json;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests.Integration
{
    [Collection("HttpListener collection")]
    public class GetRequestTests : IDisposable
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
        }

        [Fact]
        public async Task GET_Users_ReturnsListOfUsers()
        {
            var response = await _httpClient.GetAsync("http://localhost:8080/users");
            var responseBody = response.Content.ReadAsStringAsync().Result;
            Assert.Contains("Bob", responseBody);
        }

        [Theory]
        [InlineData("http://localhost:8080/notapath")]
        public async Task GET_IncorrectPath_ReturnsStatus404(string url)
        {
            var response = await _httpClient.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GET_PathWithValidUserId_ReturnsUser()
        {
            var expectedUser = new User() { Id = "1", FirstName = "Bob" };
            var expectedResponse = JsonConvert.SerializeObject(expectedUser);

            var response = await _httpClient.GetAsync("http://localhost:8080/users/1");
            var actualResponse = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedResponse, actualResponse);

        }

        [Fact]
        public async Task GET_PathWithInvalidUserId_ReturnsError()
        {
            const string expectedResponse = "User does not exist";

            var response = await _httpClient.GetAsync("http://localhost:8080/users/20");
            var actualResponse = response.Content.ReadAsStringAsync().Result;

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
