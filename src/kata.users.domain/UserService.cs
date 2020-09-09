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
            //ideally the repository should be created here as only service should know about this
        }
        // private IUserRepository UserRepository 
        // {
        //     get
        //     {
        //         if (_userRepository != null) return _userRepository;
        //         _userRepository = new DynamoDbUserRepository();
        //         return _userRepository;
        //     }
        // }


        public async Task CreateUser(string firstName)
        {
            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("Name cannot be empty");
            firstName = Formatter.FormatName(firstName);
            var user = await _userRepository.GetUserByNameAsync(firstName);
            if (user != null)
                throw new ArgumentException("Name already exists");
            await  _userRepository.AddUserAsync(firstName);
            
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsersAsync();
        }

        public async Task UpdateUser(User newUserDetails)
        {
            var userToUpdate = GetUserById(newUserDetails.Id);
            var userWithSameName = await _userRepository.GetUserByNameAsync(newUserDetails.FirstName);
            if (userWithSameName != null)
                throw new ArgumentException("A user with this name already exists");
            await _userRepository.UpdateUser( newUserDetails);
        }

        public async Task<User> GetUserById(string Id)
        {
            var user = await _userRepository.GetUserByIdAsync(Id);
            if (user == null)
                throw new ArgumentException("User does not exist");
            return user;
        }
        
        private readonly IUserRepository _userRepository;
    }
}