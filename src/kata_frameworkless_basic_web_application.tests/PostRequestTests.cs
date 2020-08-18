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
        public async Task POST_Name_ReturnsStatus200_IfAddedSuccessfully()
        {
            var userToAdd = new User() {FirstName = "Jane"};
            var jsonContent = JsonConvert.SerializeObject(userToAdd);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("http://localhost:8080/names?action=add", content);
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(userToAdd.FirstName, _httpListenerFixture.GetNameList());

            response.Dispose();
        }

        [Fact]
        public async Task POST_Name_ReturnsStatus409_IfNameAlreadyExists() 
        { 
            var userToAdd = new User() {FirstName = "Bob"};
            var jsonContent = JsonConvert.SerializeObject(userToAdd);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response1 = await _httpClient.PostAsync("http://localhost:8080/names?action=add", content);
            response1.Dispose();
            
            var response2 = await _httpClient.PostAsync("http://localhost:8080/names?action=add", content);
            var response2Body = response2.Content.ReadAsStringAsync().Result;
            
            Assert.Equal(HttpStatusCode.Conflict, response2.StatusCode);
            Assert.Contains("Name already exists", response2Body);

            response2.Dispose();
        }
        
    }
}