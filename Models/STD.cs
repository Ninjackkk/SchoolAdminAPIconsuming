using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class STD
    {
        [Key]
        public int StdId { get; set; }

        [Required]
        [StringLength(50)]
        public string StdName { get; set; } // Name of the standard/class

        // Navigation properties
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public virtual ICollection<Teacher> Teachers { get; set; } = new HashSet<Teacher>();
        public virtual ICollection<Assignment> Assignments { get; set; } = new HashSet<Assignment>();

    }
}
