using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using kata_frameworkless_web_app;
using kata_frameworkless_web_app.Repositories;
using kata_frameworkless_web_app.Services;
using Moq;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests
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
        public async Task RetrieveListOfNamesIncludingSecretUser()
        {
            var actual = await _sut.GetNameList();

            Assert.Contains("Nhan", actual);
            Assert.Contains("Bob", actual);
        }

        [Fact]
        public async Task AddNameToDatabaseIfNewName()
        {
            const string nameToAdd = "Bart";
            _sut.AddUser(nameToAdd);
            var nameList = await _sut.GetNameList();

            Assert.Contains(nameToAdd, nameList);
        }

        [Fact]
        public async Task ReturnsErrorMessageIfTryToAddNameThatAlreadyExists()
        {
            const string nameToAdd = "Nhan";
            var actual = _sut.AddUser(nameToAdd);
            
            Assert.Contains("already exists", actual.ErrorMessage);
        }
        




    }
}