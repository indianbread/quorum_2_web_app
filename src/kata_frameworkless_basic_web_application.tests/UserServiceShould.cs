using kata_frameworkless_web_app;
using Moq;

namespace kata_frameworkless_basic_web_application.tests
{
    public class UserServiceShould
    {
        public UserServiceShould()
        {
            _userService = new UserService(_mockRepository.Object);
            
        }
        private readonly Mock<IRepository> _mockRepository = new Mock<IRepository>();
        private readonly UserService _userService;
        




    }
}