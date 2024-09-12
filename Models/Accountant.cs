using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class Accountant
    {
        [Key]
        public int Accountant_id { get; set; }
        public string Accountant_name { get; set; }
        public string Accountant_email { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string role { get; set; }
    }
}
