using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using kata_frameworkless_web_app;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests
{
    public class BasicWebAppShould : IClassFixture<WebAppFixture>
    {
        private WebAppFixture _webAppFixture;
        private readonly HttpClient _client;

        public BasicWebAppShould(WebAppFixture webAppFixture)
        {
            _webAppFixture = webAppFixture;
            _client = new HttpClient();
        }

        [Fact]
        public async void ReturnMessageWithNameAndTime()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var response = await _client.GetAsync("http://localhost:8080/");
            var responseBody = await response.Content.ReadAsStringAsync();
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"Hello Nhan - the time on the server is {currentDatetime}", responseBody);
 
            response.Dispose();

        }
    }
}
