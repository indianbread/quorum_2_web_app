using System;
using System.Threading;
using kata_frameworkless_web_app;

namespace kata_frameworkless_basic_web_application.tests
{
    public class WebAppFixture : IDisposable
    {
        private readonly BasicWebApp _basicWebApp;
        private readonly Thread _webAppThread;

        public WebAppFixture()
        {
            _basicWebApp = new BasicWebApp(8080);
            _basicWebApp.Start();
            _webAppThread = new Thread(_basicWebApp.GetResponse) {IsBackground = true};
            _webAppThread.Start();
        }

        public void Dispose()
        {
            _basicWebApp.Stop();
        }
    }
}