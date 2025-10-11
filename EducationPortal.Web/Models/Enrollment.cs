namespace EducationPortal.Web.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int CourseId {  get; set; }
        public Course Course { get; set; }


    }
}
