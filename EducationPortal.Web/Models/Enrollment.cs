using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Web.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        //öğrenci
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        //ders
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }

        //ek bilgi
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    }
}
