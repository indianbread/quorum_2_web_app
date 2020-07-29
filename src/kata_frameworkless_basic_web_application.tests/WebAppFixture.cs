using System;
using System.Threading;
using kata_frameworkless_web_app;

namespace kata_frameworkless_basic_web_application.tests
{
    public class WebAppFixture : IDisposable
    {
        private readonly BasicWebApp _basicWebApp;
        
        public WebAppFixture()
        {
            _basicWebApp = new BasicWebApp();
            _basicWebApp.Start();
            _basicWebApp.ProcessRequest();
        }

        public void Dispose()
        {
           // _basicWebApp.Stop();
        }
    }
}