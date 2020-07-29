using System;
using System.Threading;
using kata_frameworkless_web_app;

namespace kata_frameworkless_basic_web_application.tests
{
    public class WebAppFixture
    {
        private readonly BasicWebApp _basicWebApp;
        private Thread _webAppThread;

        public WebAppFixture()
        {
            _basicWebApp = new BasicWebApp(new UserList());
            _webAppThread = new Thread(_basicWebApp.Start);
            _webAppThread.Start();
        }

    }
}