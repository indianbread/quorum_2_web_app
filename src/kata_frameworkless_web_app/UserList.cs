using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class UserList
    {
        public UserList()
        {
            Names = new List<string>() {"Nhan"};
        }

        public List<string> Names { get; }

        public async Task AddUser(HttpListenerRequest request, HttpListenerResponse response)
        {
            var name = GetNameFromRequestBody(request);
            if (Names.Contains(name))
            {
                response.StatusCode = 409;
                throw new ArgumentException("Error: User already exists");
            }

            Names.Add(name);
            response.StatusCode = 200;
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