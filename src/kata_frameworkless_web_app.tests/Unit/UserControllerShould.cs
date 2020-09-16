using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using kata.users.shared;
using kata_frameworkless_web_app;
using kata_frameworkless_web_app.controllers;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests.Unit
{
    public class UserControllerShould
    {

        public UserControllerShould()
        {
            mockUserService = new Mock<IService>();
            SetUpMockUserService();
            _sut = new UserController(mockUserService.Object);

        }

        [Fact]
        public async Task RespondWithUsersList_ForGetUsersRequestAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/"));
            var expectedResponse = JsonConvert.SerializeObject(testUsers);

            var actualResponse = await _sut.HandleGetRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);

        }

        [Fact]
        public async Task RespondWithUserDetails_ForGetUserRequestAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(Request => Request.Url).Returns(new Uri("http://localhost:8080/users/1"));
            var expectedResponse = JsonConvert.SerializeObject(testUser1);

            var actualResponse = await _sut.HandleGetRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);

        }

        [Fact]
        public async Task RespondWithSuccessMessage_IfValidRequest_ForCreateUserAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost:8080/users/"));
            mockRequest.Setup(request => request.GetNameFromPayload()).Returns("Hannah");
            var newUser = new User { Id = "5", FirstName = "Hannah" };

            mockUserService.Setup(us => us.CreateUserAsync(newUser.FirstName)).ReturnsAsync(newUser);
            var expectedResponse = "User added successfully";

            var actualResponse = await _sut.HandleCreateRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);

        }

        [Fact]
        public async Task RespondWithErrorMessage_IfInValidRequest_ForCreateUserAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost:8080/users/"));
            mockRequest.Setup(request => request.GetNameFromPayload()).Returns("Bob");
            mockUserService.Setup(us => us.CreateUserAsync("Bob")).ThrowsAsync(new ArgumentException("User already exists"));

            var expectedResponse = "User already exists";

            var actualResponse = await _sut.HandleCreateRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);

        }

        [Fact]
        public async Task RespondWithUpdatedUserInfo_IfValidRequest_ForUpdateUserAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost:8080/users/2"));
            mockRequest.Setup(request => request.GetNameFromPayload()).Returns("Monty");
            var updatedUser = new User { Id ="2", FirstName = "Monty"};
            mockUserService.Setup(us => us.UpdateUserAsync(It.IsAny<User>())).ReturnsAsync(updatedUser);

            var expectedResponse = JsonConvert.SerializeObject(updatedUser);

            var actualResponse = await _sut.HandleUpdateRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);

        }

        [Fact]
        public async Task RespondWithError_IfInValidId_ForUpdateUserAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost:8080/users/20"));
            mockUserService.Setup(us => us.UpdateUserAsync(It.IsAny<User>())).ThrowsAsync(new ArgumentException("User does not exist"));

            var expectedResponse = "User does not exist";

            var actualResponse = await _sut.HandleUpdateRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);
        }

        [Fact]
        public async Task RespondWithError_IfNameAlreadyExists_ForUpdateUserAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost:8080/users/3"));
            mockRequest.Setup(request => request.GetNameFromPayload()).Returns("Nhan");
            var updatedUser = new User { Id = "3", FirstName = "Monty" };
            mockUserService.Setup(us => us.UpdateUserAsync(It.IsAny<User>())).ThrowsAsync(new ArgumentException("A user with this name already exists"));

            var expectedResponse = "A user with this name already exists";

            var actualResponse = await _sut.HandleUpdateRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);
        }

        [Fact]
        public async Task RespondWithSuccessMessage_IfValidId_ForDeleteUserAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost:8080/users/2"));        
            var expectedResponse = "User successfully deleted";

            var actualResponse = await _sut.HandleDeleteRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);
        }

        [Fact]
        public async Task RespondWithErrorMessage_IfInValidId_ForDeleteUserAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost:8080/users/8"));
            mockUserService.Setup(us => us.DeleteUserAsync(It.IsAny<string>())).Throws(new ArgumentException("User does not exist"));
            var expectedResponse = "User does not exist";

            var actualResponse = await _sut.HandleDeleteRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);
        }

        [Fact]
        public async Task RespondWithErrorMessage_IfSecretUser_ForDeleteUserAsync()
        {
            var mockRequest = new Mock<IRequest>();
            mockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost:8080/users/1"));
            mockUserService.Setup(us => us.DeleteUserAsync(It.IsAny<string>())).Throws(new ArgumentException("Forbidden"));
            var expectedResponse = "Forbidden";

            var actualResponse = await _sut.HandleDeleteRequestAsync(mockRequest.Object);

            Assert.Equal(expectedResponse, actualResponse.Body);
        }



        private void SetUpMockUserService()
        {
            testUser1 = new User() { Id = "1", FirstName = "Nhan" };
            testUser2 = new User() { Id = "2", FirstName = "Bob" };
            testUser3 = new User() { Id = "3", FirstName = "Jane" };
            testUsers = new List<User>() { testUser1, testUser2 };
            mockUserService.Setup(us => us.GetUsers()).ReturnsAsync(testUsers);
            mockUserService.Setup(us => us.GetUserById("1")).ReturnsAsync(testUser1);
            mockUserService.Setup(us => us.GetUserById("2")).ReturnsAsync(testUser2);
            mockUserService.Setup(us => us.GetUserById("3")).ReturnsAsync(testUser3);
        }

        private Mock<IService> mockUserService;
        private UserController _sut;
        private User testUser1;
        private User testUser2;
        private User testUser3;
        private List<User> testUsers;
    }
}
