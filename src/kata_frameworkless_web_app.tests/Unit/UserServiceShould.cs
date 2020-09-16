using System;
using System.Linq;
using System.Threading.Tasks;
using kata_frameworkless_basic_web_application.tests.Unit.TestDoubles;
using kata.users.domain;
using kata.users.shared;
using Xunit;


namespace kata_frameworkless_basic_web_application.tests.Unit
{
    public class UserServiceShould
    {
        public UserServiceShould()
        {
            _sut = new UserService(_testUserRepository);
            _sut.SetSecretUser("Nhan");

        }
        
        private readonly IUserRepository _testUserRepository = new TestUserRepository();
        private readonly UserService _sut;
        

        [Fact]
        public async Task GetUsers_ReturnAListOfAllUsers()
        {
            var users = await _sut.GetUsers();
            var userNames = users.Select(user => user.FirstName);
            
            Assert.Contains("Nhan", userNames);
            Assert.Contains("Bob", userNames);
        }

        [Fact]
        public async Task GetUserById_ReturnsASingleUser()
        {
            var user = await _sut.GetUserById("1");

            //Assert.Equal("Nhan", user.FirstName);
            //breaks pipeline
            Assert.Equal("NotNhan", user.FirstName);

        }

        [Fact]
        public void GetUserById_ThrowsErrorIfInvalidId()
        {
            Assert.Throws<ArgumentException>(_sut.GetUserById("20").GetAwaiter().GetResult);
        }


        [Fact]
        public async Task Create_AddsNameToDatabaseIfNewName()
        {
            const string nameToAdd = "Bart";
            await _sut.CreateUserAsync(nameToAdd);
            var users = await _sut.GetUsers();
            var names = users.Select(user => user.FirstName);

            Assert.Contains(nameToAdd, names);
        }

        [Fact]
        public void Create_ReturnsErrorMessageIfTryToAddNameThatAlreadyExists()
        {
            const string nameToAdd = "Nhan";
            
            Assert.Throws<ArgumentException>(_sut.CreateUserAsync(nameToAdd).GetAwaiter().GetResult);
        }

        [Fact]
        public async Task Update_ChangesExistingNameIfGivenId()
        {
            var userToUpdate = new User()
            {
                Id = "4",
                FirstName = "Monty"
            };

            await _sut.UpdateUserAsync(userToUpdate);

            var user = await _sut.GetUserById("4");
            Assert.Equal(userToUpdate.FirstName, user.FirstName);

        }

        [Fact]
        public void Update_ThrowsErrorIfUserIdDoesNotExist()
        {
            var userToUpdate = new User()
            {
                Id = "20",
                FirstName = "Hemmingway"
            };
            
            Assert.Throws<ArgumentException>(_sut.UpdateUserAsync(userToUpdate).GetAwaiter().GetResult);
        }

        [Fact]
        public void Update_ThrowsErrorIfNameAlreadyExists()
        {
            var userToUpdate = new User()
            {
                Id = "1",
                FirstName = "Bob"
            };

            Assert.Throws<ArgumentException>(_sut.UpdateUserAsync(userToUpdate).GetAwaiter().GetResult);
        }

        [Fact]
        public async Task Update_DoesNotCreateANewResourceWithSameId()
        {
            var userToUpdate = new User()
            {
                Id = "2",
                FirstName = "MontyHall"
            };

            await _sut.UpdateUserAsync(userToUpdate);

            var allUsers = await _testUserRepository.GetUsersAsync();


            Assert.True(allUsers.Where(user => user.Id == "2").Count() == 1);

        }

        [Fact]
        public async Task Delete_RemovesUserWithValidId()
        {
            var userIdToDelete = "3";

            await _sut.DeleteUserAsync(userIdToDelete);

            Assert.Throws<ArgumentException>(_sut.GetUserById("3").GetAwaiter().GetResult);

        }

        [Fact]
        public void Delete_ThrowsErrorIfInvalidId()
        {
            var userIdToDelete = "10";

            Assert.Throws<ArgumentException>(_sut.DeleteUserAsync(userIdToDelete).GetAwaiter().GetResult);
        }

        [Fact]
        public void Delete_ThrowsErrorIfSecretUser()
        {
            var secretUser = _testUserRepository.GetUserByNameAsync("Nhan").GetAwaiter().GetResult();

            Assert.Throws<ArgumentException>(_sut.DeleteUserAsync(secretUser.Id).GetAwaiter().GetResult);
        }

    }
}