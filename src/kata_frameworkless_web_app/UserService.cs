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
        public UserService(UserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        
        private readonly UserRepository _userRepository;
        
        public IEnumerable<string> GetNameList()
        {
            return _userRepository.GetUsers();
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