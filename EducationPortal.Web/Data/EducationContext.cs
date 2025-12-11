using EducationPortal.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace EducationPortal.Web.Data
{
    
    public class EducationContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public EducationContext(DbContextOptions<EducationContext> options) : base(options)
        {
        }

        
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            

           
            var hasher = new PasswordHasher<User>();

           
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1, 
                    FullName = "Admin Kullanıcı",
                    Email = "admin@portal.com",

                
                    NormalizedEmail = "ADMIN@PORTAL.COM",
                    UserName = "admin@portal.com",
                    NormalizedUserName = "ADMIN@PORTAL.COM",

                    Role = "Admin",

                    
                    PasswordHash = hasher.HashPassword(null, "12345"),

                    SecurityStamp = Guid.NewGuid().ToString() 
                }
            );

            
            modelBuilder.Entity<CourseCategory>().HasData(
                new CourseCategory { CourseCategoryId = 1, Name = "Programlama" },
                new CourseCategory { CourseCategoryId = 2, Name = "Web Geliştirme" },
                new CourseCategory { CourseCategoryId = 3, Name = "Veri Tabanı" },
                new CourseCategory { CourseCategoryId = 4, Name = "Mobil Geliştirme" }
            );

           

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