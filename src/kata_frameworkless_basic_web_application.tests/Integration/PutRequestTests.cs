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
    public class PutRequestTests
    { 
        public PutRequestTests(HttpListenerFixture httpListenerFixture)
        {
            _httpListenerFixture = httpListenerFixture;
            _httpClient = new HttpClient();
        }
        
        private HttpClient _httpClient;
        private HttpListenerFixture _httpListenerFixture;

        [Fact]
        public async Task PUT_UpdatesNameForAValidUserId_IfNameDoesNotExist()
        {
            var newNameObject = new User() {FirstName= "Totoro"};
            var jsonContent = JsonConvert.SerializeObject(newNameObject);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var updatedUser = new User() {Id = "1", FirstName = "Totoro"};
            var updatedUserString = JsonConvert.SerializeObject(updatedUser);
            
            var response = await _httpClient.PutAsync("http://localhost:8080/users/1", content);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updatedUserString, response.Content.ReadAsStringAsync().Result);
            
            response.Dispose();
            
        }

        [Fact]
        public async Task PUT_ThrowsError_IfInvalidId()
        {
            var newNameObject = new User() { FirstName = "Kikki" };
            var jsonContent = JsonConvert.SerializeObject(newNameObject);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            var response = await _httpClient.PutAsync("http://localhost:8080/users/15", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("User does not exist", response.Content.ReadAsStringAsync().Result);

            response.Dispose();

        }

        [Fact]
        public async Task PUT_ThrowsError_IfNameAlreadyExists()
        {
            var newNameObject = new User() { FirstName = "Jane" };
            var jsonContent = JsonConvert.SerializeObject(newNameObject);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            var response = await _httpClient.PutAsync("http://localhost:8080/users/1", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal("A user with this name already exists", response.Content.ReadAsStringAsync().Result);

            response.Dispose();

        }

    }
}