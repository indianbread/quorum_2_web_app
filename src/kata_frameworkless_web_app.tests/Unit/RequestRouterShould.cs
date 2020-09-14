using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using kata_frameworkless_web_app;
using kata_frameworkless_web_app.controllers;
using kata.users.domain;
using Moq;
using Moq.Protected;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests.Unit
{
    public class RequestRouterShould
    {
        public RequestRouterShould()
        {
            _userService = Mock.Of<UserService>();
            _controllers = new List<IController>() {Mock.Of<IController>()};
            _sut = new RequestRouter(_userService, _controllers);
        }
        
        private RequestRouter _sut;
        private readonly UserService _userService;
        private readonly List<IController> _controllers;

        // [Fact]
        // public void RouteGetIndex_ToGreetingHomePage()
        // {
        //     var request = new HttpListenerRequest(); 
        //     
        //     _sut.RouteRequestAsync()
        //     
        //     
        // }
    }
}