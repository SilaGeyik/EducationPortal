namespace EducationPortal.Web.Models.ViewModels
{
    public class StudentListViewModel
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        // Öğrencinin kaç derse kayıtlı olduğu
        public int EnrolledCourseCount { get; set; }
    }
}
