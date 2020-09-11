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
        
        private readonly IUserRepository _testUserRepository = new TestUserRepository();
        private readonly UserService _sut;
        

        [Fact]
        public async Task Read_ReturnAListOfAllUsers()
        {
            var users = await _sut.GetUsers();
            var userNames = users.Select(user => user.FirstName);
            
            Assert.Contains("Nhan", userNames);
            Assert.Contains("Bob", userNames);
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
                Id = "1",
                FirstName = "Monty"
            };

            await _sut.UpdateUser(userToUpdate);

            var user = await _sut.GetUserById("1");
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
            
            Assert.Throws<ArgumentException>(_sut.UpdateUser(userToUpdate).GetAwaiter().GetResult);
        }

        [Fact]
        public void Update_ThrowsErrorIfNameAlreadyExists()
        {
            var userToUpdate = new User()
            {
                Id = "1",
                FirstName = "Nhan"
            };

            Assert.Throws<ArgumentException>(_sut.UpdateUser(userToUpdate).GetAwaiter().GetResult);
        }

        [Fact]
        public async Task Update_DoesNotCreateANewResourceWithSameId()
        {
            var userToUpdate = new User()
            {
                Id = "1",
                FirstName = "MontyHall"
            };

            await _sut.UpdateUser(userToUpdate);

            var allUsers = await _testUserRepository.GetUsersAsync();


            Assert.True(allUsers.Where(user => user.Id == "1").Count() == 1);

        }

    }
}