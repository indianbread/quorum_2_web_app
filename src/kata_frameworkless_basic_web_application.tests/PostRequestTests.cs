using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using kata_frameworkless_web_app;
using Newtonsoft.Json;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests
{
    [Collection("HttpListener collection")]
    public class PostRequestTests
    {
        public PostRequestTests(HttpListenerFixture httpListenerFixture)
        {
            _httpListenerFixture = httpListenerFixture;
            _httpClient = new HttpClient();
        }
        
        private HttpListenerFixture _httpListenerFixture;
        private readonly HttpClient _httpClient;
        
        [Fact]
        public async Task POST_Name_ReturnsStatus201_IfAddedSuccessfully()
        {
            var userToAdd = new User() {FirstName= "Jane"};
            var jsonContent = JsonConvert.SerializeObject(userToAdd);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("http://localhost:8080/users/add/", content);
            
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("User added successfully", response.Content.ReadAsStringAsync().Result);
            Assert.Equal("/users/Jane/", response.Headers.Location.ToString());
            
            response.Dispose();
        }

        [Fact]
        public async Task POST_Name_ReturnsStatus409_IfNameAlreadyExists()
        {
            await _httpListenerFixture.AddTestUser();
            var userToAdd = new User() {FirstName = "Bob"};
            var jsonContent = JsonConvert.SerializeObject(userToAdd);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("http://localhost:8080/users/add/", content);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Contains("Name already exists", responseBody);

            response.Dispose();
        }

        [Fact]
        public async Task POST_Name_ReturnsStatus400_IfPostRequestIsEmpty()
        {
            var userToAdd = new User() {FirstName = ""};
            var jsonContent = JsonConvert.SerializeObject(userToAdd);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("http://localhost:8080/users/add/", content);
            var responseBody = response.Content.ReadAsStringAsync().Result;
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Name cannot be empty", responseBody);
            
            response.Dispose();
        }
        
    }
}