namespace EducationPortal.Web.ViewModels
{
    public class StudentDashboardViewModel
    {
        public string FullName { get; set; }

        // Kartlarda kullanılacak sayılar
        public int TotalEnrolledCourses { get; set; }
        public int CompletedCourses { get; set; }
    }
}

