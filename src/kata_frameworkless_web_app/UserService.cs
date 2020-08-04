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
        public UserService(UserRepository userRepository) //instantiating the repository layer
        {
            _userRepository = userRepository;
        }
        
        private readonly UserRepository _userRepository;
        
        public async Task GetNameList(HttpListenerResponse response)
        {
            var names = _userRepository.GetNames();
            var nameList = ResponseFormatter.GenerateNamesList(names);
            await ResponseFormatter.GenerateResponseBody(response, nameList);
        }
        
        private static string GetNameFromRequestBody(HttpListenerRequest request)
        {
            var body = request.InputStream;
            var reader = new StreamReader(body, Encoding.UTF8);
            var name = reader.ReadToEnd();
            reader.Close();
            return name;
        }

        
    }
}