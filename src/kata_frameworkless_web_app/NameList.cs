using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class NameList
    {
        public NameList()
        {
            Names = new List<string>() {"Nhan"};
        }

        public List<string> Names { get; }

        public async Task AddName(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                var name = GetNameFromRequestBody(request);
                if (Names.Contains(name))
                {
                    response.StatusCode = (int) HttpStatusCode.Conflict;
                    throw new ArgumentException("Error: Name already exists");
                }
                Names.Add(name);
                response.StatusCode= (int)HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                await ResponseFormatter.GenerateResponseBody(response, e.Message);
            }
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