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

        public async Task CreateUser(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("Name cannot be empty");
            firstName = Formatter.FormatName(firstName);
            var user = await _userRepository.FindUserByNameAsync(firstName);
            if (user != null)
                throw new ArgumentException("Name already exists");
            await  _userRepository.AddUserAsync(firstName);
            
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsersAsync();
        }
    }
}