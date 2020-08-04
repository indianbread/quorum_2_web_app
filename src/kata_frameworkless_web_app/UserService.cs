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
        
        public async Task<List<string>> GetNameList()
        {
            return await _userRepository.GetUsers();
        }


        public async Task AddName(string name, HttpListenerResponse response)
        {
            try
            {
                if (UserExists(name))
                {
                    response.StatusCode = (int) HttpStatusCode.Conflict;
                    throw new ArgumentException("Error: Name already exists");
                }
                response.StatusCode= (int)HttpStatusCode.OK;
                await ResponseFormatter.GenerateResponseBody(response, JsonSerializer.Serialize(_userRepository.GetUsers()));
            }
            catch (Exception e)
            {
                await ResponseFormatter.GenerateResponseBody(response, e.Message);
            }
        }

        private bool UserExists(string name)
        {
            return !(_userRepository.FindUserByName(name) == null);
        }
    }
}