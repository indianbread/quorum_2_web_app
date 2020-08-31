using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kata_frameworkless_web_app.Repositories;

namespace kata_frameworkless_web_app.Services
{
    public class UserService
    {
        public UserService(IRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        
        private readonly IRepository _userRepository;

        public void AddSecretUserName(string name)
        {
            _userRepository.AddUser(name);
        }

        public async Task<IEnumerable<string>> GetNameList()
        {
            var users = await _userRepository.GetUsers();
            return users.Select(user => user.FirstName);
            
        }
        
        public RequestResult AddUser(string name)
        {
 
            if (string.IsNullOrWhiteSpace(name))
            {
                return RequestResult.CreateError("Name cannot be empty", HttpStatusCode.BadRequest);
            }
            if (UserExists(name))
            {
                return RequestResult.CreateError("Name already exists", HttpStatusCode.Conflict);
            }
            _userRepository.AddUser(name);
            return RequestResult.CreateSuccess("User added successfully", HttpStatusCode.Created);
        }
        
        private bool UserExists(string name)
        {
            return _userRepository.FindUserByName(name) != null;
        }
    }
}