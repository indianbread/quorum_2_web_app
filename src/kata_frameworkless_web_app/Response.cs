using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kata_frameworkless_web_app
{
    public class Response
    {
        public static async Task GenerateBodyAsync(HttpListenerResponse response, string responseString)
        {
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            await response.OutputStream.DisposeAsync();
        }
        
    }
}