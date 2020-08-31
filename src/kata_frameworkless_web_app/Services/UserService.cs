using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using kata_frameworkless_web_app.Repositories;

namespace kata_frameworkless_web_app.Services
{
    public class UserService
    {
        public UserService(IUserRepository userUserRepository) 
        {
            _userUserRepository = userUserRepository;
        }
        
        private readonly IUserRepository _userUserRepository;

        public void AddSecretUserName(string name)
        {
            _userUserRepository.AddUser(name);
        }

        public async Task<IEnumerable<string>> GetNameList()
        {
            var users = await _userUserRepository.GetUsersAsync();
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
            _userUserRepository.AddUser(name);
            return RequestResult.CreateSuccess("User added successfully", HttpStatusCode.Created);
        }
        
        private bool UserExists(string name)
        {
            return _userUserRepository.FindUserByName(name) != null;
        }
    }
}