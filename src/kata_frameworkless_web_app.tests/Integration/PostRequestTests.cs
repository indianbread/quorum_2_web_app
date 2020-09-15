using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using kata.users.shared;
using Newtonsoft.Json;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests.Integration
{
    [Collection("HttpListener collection")]
    public class PostRequestTests : IDisposable
    {
        public PostRequestTests(HttpListenerFixture httpListenerFixture)
        {
            _httpListenerFixture = httpListenerFixture;
            _httpClient = new HttpClient();
        }
        
        private HttpListenerFixture _httpListenerFixture;
        private readonly HttpClient _httpClient;
        
        [Fact]
        public async Task POST_Name_ReturnsStatus200_IfAddedSuccessfully()
        {
            var userToAdd = new User() { FirstName = "Mary" };
            var jsonContent = JsonConvert.SerializeObject(userToAdd);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:8080/users", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("User added successfully", response.Content.ReadAsStringAsync().Result);
        }

        [Fact]
        public async Task POST_Name_ReturnsError_IfNameAlreadyExists()
        {
            var userToAdd = new User() { FirstName = "Bob" };
            var jsonContent = JsonConvert.SerializeObject(userToAdd);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:8080/users", content);
            var responseBody = response.Content.ReadAsStringAsync().Result;

            Assert.Contains("Name already exists", responseBody);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task POST_Name_ReturnsError_IfPostRequestIsEmpty()
        {
            var userToAdd = new User() { FirstName = "" };
            var jsonContent = JsonConvert.SerializeObject(userToAdd);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:8080/users", content);
            var responseBody = response.Content.ReadAsStringAsync().Result;

            Assert.Contains("Name cannot be empty", responseBody);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            try
            {
                var userToDelete = _httpListenerFixture.UserRepository.GetUserByNameAsync("Mary").GetAwaiter().GetResult();
                _httpListenerFixture.UserRepository.DeleteUserAsync(userToDelete).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
    }
}