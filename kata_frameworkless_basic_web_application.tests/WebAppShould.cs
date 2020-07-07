using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using kata_frameworkless_web_app;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests
{
    public class WebAppShould
    {

        [Fact]
        public void ReturnMessageWithNameAndTime()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            
            new Thread(() => 
            {
                Thread.CurrentThread.IsBackground = true;
                var basicWebApp = new BasicWebApp(8080);
                basicWebApp.GetResponse();
            }).Start();
            
            var request =
                (HttpWebRequest)WebRequest.Create("http://localhost:8080/");
            var response = (HttpWebResponse) request.GetResponse();
            var dataStream = response.GetResponseStream();
            var reader = new StreamReader(dataStream);
            var responseString = reader.ReadToEnd();
            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains($"Hello Nhan - the time on the server is {currentDatetime}", responseString);
            
            reader.Close();
            dataStream.Close();
            response.Close();

        }
    }
}

//before test: start server
// after test: close server
//check if need to dispose server manually