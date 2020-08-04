using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace kata_frameworkless_web_app
{
    public class ResponseFormatter
    {
        public static string GetGreeting(List<string> names)
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var namesString = string.Join(", ", names.Take(names.Count - 1)) + (names.Count <= 1 ? "" : " and ") + names.LastOrDefault();
            return "Hello " + namesString + " - the time on the server is " + currentDatetime;
        }

        public static string GenerateNamesListBody(IEnumerable<string> names)
        {
            var nameList = "Name List:" + Environment.NewLine;
            nameList =
                names.Aggregate(nameList, (current, name) => current + (name + Environment.NewLine));
            return nameList;
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