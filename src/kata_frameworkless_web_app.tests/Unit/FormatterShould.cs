using System;
using System.Collections.Generic;
using kata.users.shared;
using Xunit;

namespace kata_frameworkless_basic_web_application.tests.Unit
{
    public class FormatterShould
    {
        [Fact]
        public void DisplayGreetingWithDateAndTime_ForOneName()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var names = new List<string>() { "Nhan"};
            var expectedGreeting = "Hello Nhan - the time on the server is " + currentDatetime;
            
            var actual = Formatter.FormatGreeting(names);
            
            Assert.Equal(expectedGreeting, actual);
        }

        [Fact]
        public void FormatsGreeting_ForTwoNames()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var names = new List<string>() { "Nhan", "Bob"};
            var expectedGreeting = "Hello Nhan and Bob - the time on the server is " + currentDatetime;

            var actual = Formatter.FormatGreeting(names);
            
            Assert.Equal(expectedGreeting, actual);
        }
        
        [Fact]
        public void FormatsGreeting_ForThreeNamesOrMore()
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var names = new List<string>() { "Nhan", "Bob", "Jane"};
            var expectedGreeting = "Hello Nhan, Bob and Jane - the time on the server is " + currentDatetime;

            var actual = Formatter.FormatGreeting(names);
            
            Assert.Equal(expectedGreeting, actual);
        }
    }
}