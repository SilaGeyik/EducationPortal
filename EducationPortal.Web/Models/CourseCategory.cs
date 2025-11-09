using System.ComponentModel.DataAnnotations;

namespace EducationPortal.Web.Models
{
    public class CourseCategory
    {
        [Key]
        public int CourseCategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
}

