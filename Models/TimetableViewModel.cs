using System.ComponentModel.DataAnnotations;

namespace SchoolAdminAPIconsuming.Models
{
    public class TimetableViewModel
    {

        [Required]
        public string TimetableName { get; set; }

        public IFormFile TimetableFile { get; set; }

        [Required]
        public int StdId { get; set; }
    }
}
