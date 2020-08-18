using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using kata_frameworkless_web_app;
using Moq;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests
{
    public class UserServiceShould
    {
        public UserServiceShould()
        {
            _sut = new UserService(_testRepository);

        }
        
        private readonly IRepository _testRepository = new TestUserRepository();
        private readonly UserService _sut;
        

        [Fact]
        public void RetrieveListOfNamesIncludingSecretUser()
        {
            var actual = _sut.GetNameList();

            Assert.Contains("Nhan", actual);
            Assert.Contains("Bob", actual);
        }

        [Fact]
        public void AddNameToDatabaseIfNewName()
        {
            const string nameToAdd = "Bart";
            var actual = _sut.AddName(nameToAdd);

            Assert.Contains(nameToAdd, _sut.GetNameList());
        }

        [Fact]
        public async Task ReturnsErrorMessageIfTryToAddNameThatAlreadyExists()
        {
            const string nameToAdd = "Nhan";
            var actual = _sut.AddName(nameToAdd);
            
            Assert.Contains("already exists", actual);
        }
        




    }
}