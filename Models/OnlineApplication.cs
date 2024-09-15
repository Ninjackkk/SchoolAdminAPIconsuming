using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class OnlineApplication
    {
        [Key]
        public int OnlineApplicationID { get; set; }
        public string StudentName { get; set; }
        public string ApplyingForSTD { get; set; }
        public string Address { get; set; }
        public string Parent_Name { get; set; }
        public string Parent_Email { get; set; }
    }
}
