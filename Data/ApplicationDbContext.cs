using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using SchoolAdminAPIconsuming.Models;


namespace SchoolAdminAPIconsuming.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Accountant> Accountants { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<SystemAdmin> SystemAdmins { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<OnlineApplication> OnlineApplications { get; set; }

        public DbSet<STD> STDs { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<Timetable> Timetables { get; set; }

        public DbSet<AssignmentResponse> AssignmentResponses { get; set; }

        public DbSet<AcademicReport> AcademicReports { get; set; }

        public DbSet<BookIssuance> BookIssuances { get; set; }

        public DbSet<Book> Books { get; set; }





    }
}
