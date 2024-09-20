using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class Librarian
    {
        [Key]
        public int LibrarianId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }  // For login
        public string Password { get; set; }
        public double MonthlySalary { get; set; }
        public DateTime HireDate { get; set; }
        public string role { get; set; }
    }
}
