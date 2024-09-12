using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Qualification { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        // New Properties
        public string UserId { get; set; }
        public string Password { get; set; }
        public double MonthlySalary { get; set; }
        public string STD { get; set; }
        public string role { get; set; }
    }
}
