using System.Collections.Generic;
using System.Net;

namespace kata_frameworkless_web_app
{
    public class Greeting
    {
        public Greeting(HttpListener listener)
        {
            _users = new List<string>() {"Nhan"};
            _listener = listener;
        }
        private List<string> _users;
        private HttpListener _listener;
    }
}