using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using kata.users.shared;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests.Integration
{
    [Collection("HttpListener collection")]
    public class DeleteRequestTests : IDisposable
    {
        public DeleteRequestTests(HttpListenerFixture httpListenerFixture)
        {
            _httpListenerFixture = httpListenerFixture;
            _httpClient = new HttpClient();
            Thread.Sleep(1000);
        }

        private HttpListenerFixture _httpListenerFixture;
        private HttpClient _httpClient;

        [Fact]
        public async Task Delete_DeletesUserWithValidId()
        {
            Thread.Sleep(2000);

            var response = await _httpClient.DeleteAsync("http://localhost:8080/users/3");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
        }

        [Fact]
        public async Task Delete_ReturnsErrorIfInvalidId()
        {
            Thread.Sleep(3000);

            var response = await _httpClient.DeleteAsync("http://localhost:8080/users/20");
            
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            
        }

        [Fact]
        public async Task Delete_ReturnsForbiddenIfDeleteSecretUser()
        {
            var secretUser = await _httpListenerFixture.UserRepository.GetUserByNameAsync("Nhan");

            var response = await _httpClient.DeleteAsync($"http://localhost:8080/users/{secretUser.Id}");

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);

        }

        public void Dispose()
        {
            var userToRestore = new User()
            {
                Id = "3",
                FirstName = "John"
            };

            _httpListenerFixture.UserRepository.CreateUserAsync(userToRestore).GetAwaiter().GetResult();
            _httpClient.Dispose();
        }
    }
}
