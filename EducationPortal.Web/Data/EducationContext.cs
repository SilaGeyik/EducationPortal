using Microsoft.EntityFrameworkCore;
using EducationPortal.Web.Models;

namespace EducationPortal.Web.Data
{
    public class EducationContext : DbContext
    {
        public EducationContext(DbContextOptions<EducationContext> options)
            : base(options)
        {
        }

        // Veritabanındaki tabloları temsil eden DbSet'ler
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
    }
}
