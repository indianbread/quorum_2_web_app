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
            _names = new List<string>() {AwsSecretManager.GetSecret()};
        }
        
        private readonly IRepository _userRepository;
        private readonly List<string> _names;

        public IEnumerable<string> GetNameList()
        {
            var storedUsers= _userRepository.GetUsers();
            _names.AddRange(storedUsers);
            return _names;
        }


        public async Task<string> AddName(string name)
        {
            try
            {
                if (UserExists(name))
                {
                    throw new ArgumentException("Error: Name already exists");
                }
                _userRepository.AddUser(name);
                return JsonSerializer.Serialize(_userRepository.GetUsers());
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private bool UserExists(string name)
        {
            return _userRepository.FindUserByName(name) != null;
        }
    }
}