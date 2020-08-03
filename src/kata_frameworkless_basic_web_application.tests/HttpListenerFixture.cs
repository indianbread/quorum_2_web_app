using System;
using System.Threading;
using kata_frameworkless_web_app;

namespace kata_frameworkless_basic_web_application.tests
{
    public class HttpListenerFixture
    {
        private readonly BasicWebApp _basicWebApp;
        private Thread _webAppThread;
        private NameList _nameList = new NameList();

        public HttpListenerFixture()
        {
            _basicWebApp = new BasicWebApp(new NameController(_nameList), _nameList );
            _webAppThread = new Thread(_basicWebApp.Start);
            _webAppThread.Start();
        }

    }
}