using EducationPortal.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Web.Data
{
    public class EducationContext : DbContext
    {
        public EducationContext(DbContextOptions<EducationContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId);
        }
    }
}
