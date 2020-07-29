using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class ResponseFormatter
    {
        public static string GetGreeting(List<string> names)
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var usersString = names.First();
            if (names.Count <= 1) return "Hello " + usersString + " - the time on the server is " + currentDatetime;;
            for (var i = 1; i < names.Count - 1; i++)
            {
                usersString += ", " + names[i];
            }
            usersString += " and " + names.Last();

            return "Hello " + usersString + " - the time on the server is " + currentDatetime;
        }
        
        public static async Task GenerateResponseBody(HttpListenerResponse response, string responseString)
        {
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
        
    }
}