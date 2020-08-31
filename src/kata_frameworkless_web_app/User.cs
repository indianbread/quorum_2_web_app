using System.ComponentModel.DataAnnotations.Schema;
using Amazon.DynamoDBv2.DataModel;

namespace kata_frameworkless_web_app
{
    //[Table("Users", Schema = "dbo")]
    [DynamoDBTable("User")]
    public class User
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        public string FirstName { get; set; }

        
    }
}