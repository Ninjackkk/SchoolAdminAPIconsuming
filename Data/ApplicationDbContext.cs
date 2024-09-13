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



    }
}
