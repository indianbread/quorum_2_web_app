using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace kata_frameworkless_web_app
{
    public class Request
    {
        public static string GetNameFromPayload(HttpListenerRequest request)
        {
            var body = request.InputStream;
            using (var reader = new StreamReader(body, Encoding.UTF8))
            {
                var data = reader.ReadToEnd();
                var user = JObject.Parse(data);
                return (user["FirstName"] ?? "").Value<string>();
            }
        }
        
    }
}