using System;
using System.Net;
using System.Net.Http;
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
        }

        private HttpListenerFixture _httpListenerFixture;
        private HttpClient _httpClient;

        [Fact]
        public async Task Delete_DeletesUserWithValidId()
        {
            using (var response = await _httpClient.DeleteAsync("http://localhost:8080/users/3"))
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_ReturnsErrorIfInvalidId()
        {
            using (var response = await _httpClient.DeleteAsync("http://localhost:8080/users/20"))
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Delete_ReturnsForbiddenIfDeleteSecretUser()
        {
            var secretUser = await _httpListenerFixture.UserRepository.GetUserByNameAsync("Nhan");
            using (var response = await _httpClient.DeleteAsync($"http://localhost:8080/users/{secretUser.Id}"))
            {
                Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
            }
        }

        public void Dispose()
        {
            var userToRestore = new User()
            {
                Id = "3",
                FirstName = "John"
            };

            _httpListenerFixture.UserRepository.CreateUserAsync(userToRestore).GetAwaiter().GetResult();
        }
    }
}
