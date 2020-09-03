using System.ComponentModel.DataAnnotations.Schema;
using Amazon.DynamoDBv2.DataModel;

namespace kata_frameworkless_web_app
{
    //[Table("Users", Schema = "dbo")]
    [DynamoDBTable("NhanUser")]
    public class User
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string FirstName { get; set; }

        
    }
}