using System;
using System.Collections.Generic;
using System.Linq;


namespace kata_frameworkless_web_app
{
    public static class Formatter
    {
        public static string FormatGreeting(List<string> names)
        {
            var currentDatetime = DateTime.Now.ToString("hh:mm tt on dd MMMM yyyy");
            var namesString = string.Join(", ", names.Take(names.Count - 1)) + (names.Count <= 1 ? "" : " and ") + names.LastOrDefault();
            return "Hello " + namesString + " - the time on the server is " + currentDatetime;
        }
        
        public static string FormatName(string name)
        {
            return name.Substring(0, 1).ToUpper() + name.Substring(1, name.Length - 1);
        }
        
        public static string FormatControllerName(string resourceGroup)
        {
            var resourceGroupName = resourceGroup.Substring(0, resourceGroup.Length - 1);
            return Formatter.FormatName(resourceGroupName);
        }
    }
}