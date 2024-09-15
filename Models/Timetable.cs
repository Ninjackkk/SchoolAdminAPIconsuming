using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class Timetable
    {
        public int TimetableId { get; set; }

        [Required]
        public string? TimetableName { get; set; }

        public string? TimetableFile { get; set; } // Path to the uploaded file

        [Required]
        public int? StdId { get; set; }

        public virtual STD STD { get; set; } // Navigation property to STD
    }
}
