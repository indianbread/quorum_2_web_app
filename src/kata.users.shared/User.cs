using System;

namespace kata.users.shared
{
    public class User //TODO: how to convert to a type that dynamodb db understands?
    {
        public string Id { get; set; }
        
        public string FirstName { get; set; }
    }
}