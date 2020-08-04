using System.ComponentModel.DataAnnotations.Schema;

namespace kata_frameworkless_web_app
{
    [Table("Users", Schema = "dbo")]
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        
    }
}