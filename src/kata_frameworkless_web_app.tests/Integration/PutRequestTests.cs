using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PutRequestTests : IDisposable
    { 
        public PutRequestTests(HttpListenerFixture httpListenerFixture)
        {
            _httpListenerFixture = httpListenerFixture;
            _httpClient = new HttpClient();
        }
        
        private HttpClient _httpClient;
        private HttpListenerFixture _httpListenerFixture;

        [Fact]
        public async Task PUT_ChangesNameForAValidUserId_IfNameDoesNotExist()
        {
            var newNameObject = new User() { FirstName = "Totoro" };
            var jsonContent = JsonConvert.SerializeObject(newNameObject);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var updatedUser = new User() { Id = "5", FirstName = "Totoro" };
            var updatedUserString = JsonConvert.SerializeObject(updatedUser);

            HttpResponseMessage response = await _httpClient.PutAsync("http://localhost:8080/users/5", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updatedUserString, response.Content.ReadAsStringAsync().Result);
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

        }

        [Fact]
        public async Task PUT_ThrowsError_IfNameAlreadyExists()
        {
            var newNameObject = new User() { FirstName = "Jane" };
            var jsonContent = JsonConvert.SerializeObject(newNameObject);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");


            var response = await _httpClient.PutAsync("http://localhost:8080/users/1", content);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal("A user with this name already exists", response.Content.ReadAsStringAsync().Result);

        }

        public void Dispose()
        {
            var userToRestore = new User() { Id = "5", FirstName = "Anna" };
            var jsonContent = JsonConvert.SerializeObject(userToRestore);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = _httpClient.PutAsync("http://localhost:8080/users/5", content).GetAwaiter().GetResult();
            _httpClient.Dispose();

        }
    }
}