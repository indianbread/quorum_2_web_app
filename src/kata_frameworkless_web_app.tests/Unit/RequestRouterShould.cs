using System.Collections.Generic;
using System.Threading.Tasks;
using kata_frameworkless_web_app;
using kata_frameworkless_web_app.controllers;
using Moq;
using Xunit;
using System;
using kata_frameworkless_basic_web_application.tests.Unit.TestDoubles;
using kata.users.shared;


namespace kata_frameworkless_basic_web_application.tests.Unit
{
    public class RequestRouterShould
    {
        public RequestRouterShould()
        {
            var mockUserService = new Mock<IService>();
            mockIndexController = new MockIndexController();
            mockUserController = new MockUserController();
            controllers = new List<IController> { mockIndexController, mockUserController };
            _sut = new RequestRouter(mockUserService.Object, controllers);
        }
        
        private RequestRouter _sut;
        private IController mockIndexController;
        private IController mockUserController;
        private IList<IController> controllers;

        [Fact]
        public async Task RouteGetIndex_ToIndexControllerGetAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("GET");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/"));

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains("MockIndexController HandleGetRequestAsync called", actualResponse.Body);

         }

        [Fact]
        public async Task RouteGetUsers_ToUserControllerGetAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("GET");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/"));

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains("MockUserController HandleGetRequestAsync called", actualResponse.Body);
        }

        [Fact]
        public async Task RouteGetUser_ToUserControllerGetAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("GET");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/1"));

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains("MockUserController HandleGetRequestAsync called", actualResponse.Body);
        }

        [Fact]
        public async Task RouteDeleteUser_ToUserControllerDeleteAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("DELETE");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/1"));

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains("MockUserController HandleDeleteRequestAsync called", actualResponse.Body);
        }

        [Fact]
        public async Task RoutePutUser_ToUserControllerPutAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("PUT");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/1"));

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains("MockUserController HandlePutRequestAsync called", actualResponse.Body);
        }

        [Fact]
        public async Task RoutePostUser_ToUserControllerPostAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.HttpMethod).Returns("POST");
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/"));

            var actualResponse = await _sut.RouteRequestAsync(mockRequest.Object);

            Assert.Contains("MockUserController HandlePostRequestAsync called", actualResponse.Body);
        }
    }
}