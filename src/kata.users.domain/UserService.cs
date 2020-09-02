using System;
using kata.users.shared;

namespace kata.users.domain
{
    public class UserService
    {
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private readonly IUserRepository _userRepository;

        public void CreateUser(CreateUserRequest createUserRequest)
        {
            if (createUserRequest == null)
                throw new ArgumentException("Request cannot be empty");
            if (string.IsNullOrEmpty(createUserRequest.FirstName))
                throw new ArgumentException("Name cannot be empty");
            _userRepository.AddUserAsync(createUserRequest.FirstName);
        }
    }
}