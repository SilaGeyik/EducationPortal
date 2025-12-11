using System;

namespace EducationPortal.Web.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalStudents { get; set; }      // Toplam öğrenci
        public int TotalCourses { get; set; }       // Toplam kurs
        public int TotalEnrollments { get; set; }   // Toplam kayıt
    }
}
