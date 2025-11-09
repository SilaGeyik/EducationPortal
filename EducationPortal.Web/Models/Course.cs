using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Web.Models
{
    public class Course
    {
        public int CourseId { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = "";

        [Range(0, 20)]
        public int Credits { get; set; }

        [Required, StringLength(150)]
        public string Instructor { get; set; } = "";

        // ZORUNLU FK
        [Required]
        public int CourseCategoryId { get; set; }

        // Navigation
        public CourseCategory? CourseCategory { get; set; }
    }
}
