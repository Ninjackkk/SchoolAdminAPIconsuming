using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class SystemAdmin
    {
        [Key]
        public int SystemAdmin_id { get; set; }
        public string SystemAdmin_name { get; set; }
        public string SystemAdmin_email { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string role { get; set; }
    }
}
