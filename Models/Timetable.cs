using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class Timetable
    {
        public int TimetableId { get; set; }

        [Required]
        public string? TimetableName { get; set; }

        public string? TimetableFile { get; set; } // Path to the uploaded file

        public string? STD { get; set; }

    }
}
