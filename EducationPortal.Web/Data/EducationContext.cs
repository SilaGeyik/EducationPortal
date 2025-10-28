using EducationPortal.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Web.Data
{
    public class EducationContext : DbContext
    {
        // Constructor: dışarıdan alınan ayarlarla DbContext'i başlatır
        public EducationContext(DbContextOptions<EducationContext> options) : base(options)
        {
        }

        // Veritabanındaki tabloları temsil eden DbSet'ler
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        // Veritabanı oluşturulurken başlangıç verileri (Seed Data)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User tablosu için varsayılan admin hesabı ekleniyor
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FullName = "Admin Kullanıcı",
                    Email = "admin@portal.com",
                    Password = "12345",
                    Role = "Admin"
                }
            );

            // İlişkiler (Enrollment -> User ve Course)
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
