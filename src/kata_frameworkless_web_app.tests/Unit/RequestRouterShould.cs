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
            testUser1 = new User { FirstName = "Nhan", Id = "1" };
            testUser2 = new User { FirstName = "Bob", Id = "2" };
            testUsers = new List<User> { testUser1, testUser2 };
            var mockUserService = new Mock<IService>();
            mockUserService.Setup(service => service.GetUsers()).ReturnsAsync(testUsers);
            _sut = new RequestRouter(mockUserService.Object);
        }
        
        private RequestRouter _sut;
        private User testUser1;
        private User testUser2;
        private List<User> testUsers;

        [Fact]
        public async Task RouteGetIndex_ToGreetingHomePageAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("GET");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/"));

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains("Hello Nhan and Bob - the time on the server is", actualResponse.Body);

         }

        [Fact]
        public async Task RouteGetUsers_ToUsersPageAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("GET");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/"));
            var expectedResponseBody = JsonConvert.SerializeObject(testUsers);

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains(expectedResponseBody, actualResponse.Body);

        }

        [Fact]
        public async Task RouteGetUser_ToUserPageAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("GET");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/1"));
            var expectedResponseBody = JsonConvert.SerializeObject(testUser1);

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains(expectedResponseBody, actualResponse.Body);
        }
    }
}