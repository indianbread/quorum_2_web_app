using System;
using System.Linq;
using System.Threading.Tasks;
using kata.users.domain;
using kata.users.shared;
using Xunit;
using IUserRepository = kata.users.shared.IUserRepository;

namespace kata_frameworkless_basic_web_application.tests.Unit
{
    public class UserServiceShould
    {
        public UserServiceShould()
        {
            _sut = new UserService(_testUserRepository);

        }
        
        private readonly IUserRepository _testUserRepository = new TestUserUserRepository();
        private readonly UserService _sut;
        

        [Fact]
        public async Task GetUsers_IncludeSecretUserAndTestUser()
        {
            var users = await _sut.GetUsers();
            var userNames = users.Select(user => user.FirstName);
            
            Assert.Contains("Nhan", userNames);
            Assert.Contains("Bob", userNames);
        }

        [Fact]
        public async Task AddNameToDatabaseIfNewName()
        {
            const string nameToAdd = "Bart";
            var createUserRequest = new CreateUserRequest() {FirstName = nameToAdd};
            await _sut.CreateUser(createUserRequest);
            var users = await _sut.GetUsers();
            var names = users.Select(user => user.FirstName);

            Assert.Contains(nameToAdd, names);
        }

        [Fact]
        public void ReturnsErrorMessageIfTryToAddNameThatAlreadyExists()
        {
            const string nameToAdd = "Nhan";
            var createUserRequest = new CreateUserRequest() {FirstName = nameToAdd};

            Assert.Throws<Exception>(_sut.CreateUser(createUserRequest).GetAwaiter().GetResult);
        }
        




    }
}