namespace SchoolAdminAPIconsuming.Models
{
    public class AcademicReport
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public double AttendanceAverage { get; set; }
        public double AssignmentAverage { get; set; }
        public double Behavior { get; set; }
        public string Remarks { get; set; }
        public double Overall { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
