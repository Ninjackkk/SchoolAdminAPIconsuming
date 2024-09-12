using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Parent_Email { get; set; }
        public string FeesStatus { get; set; }
        public string role { get; set; }
    }
}
