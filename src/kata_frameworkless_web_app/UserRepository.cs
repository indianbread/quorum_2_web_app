using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace kata_frameworkless_web_app
{
    public class UserRepository
    {
        public UserRepository(DbContext usersDatabase)
        {
            _usersDatabase = usersDatabase;
        }
        private readonly DbContext _usersDatabase;
        
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
                await ResponseFormatter.GenerateResponseBody(response, JsonSerializer.Serialize(Names));
            }
            catch (Exception e)
            {
                await ResponseFormatter.GenerateResponseBody(response, e.Message);
            }
        }

    }
}