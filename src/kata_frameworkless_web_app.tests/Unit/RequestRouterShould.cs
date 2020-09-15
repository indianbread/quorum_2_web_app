using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using kata_frameworkless_web_app;
using kata_frameworkless_web_app.controllers;
using kata.users.domain;
using Moq;
using Moq.Protected;
using Xunit;
using System;
using kata.users.shared;
using Newtonsoft.Json;

namespace kata_frameworkless_basic_web_application.tests.Unit
{
    public class RequestRouterShould
    {
        public RequestRouterShould()
        {
            testUser = new User { FirstName = "Nhan", Id = "1" };
            var mockUserService = new Mock<IService>();
            mockUserService.Setup(service => service.GetUsers()).ReturnsAsync(new List<User> { testUser });
            _sut = new RequestRouter(mockUserService.Object);
        }
        
        private RequestRouter _sut;
        private User testUser;

        [Fact]
        public async Task RouteGetIndex_ToGreetingHomePageAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("GET");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/"));

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains("Hello Nhan - the time on the server is", actualResponse.Body);

         }

        [Fact]
        public async Task RouteGetUsers_UsersPageAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("GET");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/"));
            var expectedResponseBody = JsonConvert.SerializeObject(testUser);

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains(expectedResponseBody, actualResponse.Body);

        }
    }
}