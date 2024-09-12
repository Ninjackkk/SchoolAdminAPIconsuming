using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class Librarian
    {
        [Key]
        public int Librarian_id { get; set; }
        public string Librarian_name { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string role { get; set; }
    }
}
