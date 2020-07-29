using System;
using System.Collections.Generic;
using System.Linq;

namespace kata_frameworkless_web_app
{
    public class ResponseFormatter
    {
        public static string GetGreeting(List<string> names)
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var usersString = names.First();
            if (names.Count <= 1) return "Hello " + usersString + " - the time on the server is " + currentDatetime;;
            for (var i = 1; i < names.Count - 1; i++)
            {
                usersString += ", " + names[i];
            }
            usersString += " and " + names.Last();

            return "Hello " + usersString + " - the time on the server is " + currentDatetime;
        }
        
    }
}