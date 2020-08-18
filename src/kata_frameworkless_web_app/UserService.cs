using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class UserService
    {
        public UserService(IRepository userRepository) 
        {
            _userRepository = userRepository;
            _names = new List<string>();
        }
        
        private readonly IRepository _userRepository;
        private readonly List<string> _names;

        public void AddSecretUserName(string name)
        {
            _names.Add(name);
        }

        public IEnumerable<string> GetNameList()
        {
            var storedUsers= _userRepository.GetUsers();
            _names.AddRange(storedUsers);
            return _names;
        }


        public RequestResult AddName(string name)
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
            return RequestResult.CreateSuccess("User added successfully");
        }
        
        private bool UserExists(string name)
        {
            return _userRepository.FindUserByName(name) != null;
        }
    }
}