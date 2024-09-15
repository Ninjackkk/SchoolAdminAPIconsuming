namespace SchoolAdminAPIconsuming.Models
{
    public class AssignmentViewModel
    {
        public string AssignmentName { get; set; }
        public DateTime AssignmentDate { get; set; }
        public DateTime Deadline { get; set; }
        public IFormFile AssignmentFile { get; set; }
        public string GivenBy { get; set; }
        public int StdId { get; set; }
    }

}
