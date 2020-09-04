using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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

        public async Task CreateUser(CreateUserRequest createUserRequest)
        {
            if (createUserRequest == null)
                throw new ArgumentException("Request cannot be empty");
            if (string.IsNullOrEmpty(createUserRequest.FirstName))
                throw new ArgumentException("Name cannot be empty");
            var user = await _userRepository.FindUserByName(createUserRequest.FirstName);
            if (user != null)
                throw new ArgumentException("User already exists");
            await  _userRepository.AddUserAsync(createUserRequest.FirstName);
            
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsersAsync();
        }
    }
}