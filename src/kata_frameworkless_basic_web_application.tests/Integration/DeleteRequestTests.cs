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

            using (var response2 = await _httpClient.GetAsync("http://localhost:8080/users/3"))
            {
                Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);
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
