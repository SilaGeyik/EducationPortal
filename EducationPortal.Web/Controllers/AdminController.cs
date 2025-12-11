using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using EducationPortal.Web.Repositories;
using EducationPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICourseRepository _courseRepository;
        private readonly EducationContext _context;

        public AdminController(
            UserManager<User> userManager,
            ICourseRepository courseRepository,
            EducationContext context)
        {
            _userManager = userManager;
            _courseRepository = courseRepository;
            _context = context;
        }

        // Dashboard
        public IActionResult Index()
        {
            // Toplam öğrenci 
            var totalStudents = _userManager.Users
                                            .Count(u => u.Role == "Student");

            // Kurslar repository'den
            var courses = _courseRepository.GetAll();
            var totalCourses = courses.Count;

            // Toplam kayıt 
            var totalEnrollments = _context.Enrollments.Count();

            var model = new AdminDashboardViewModel
            {
                TotalStudents = totalStudents,
                TotalCourses = totalCourses,
                TotalEnrollments = totalEnrollments
            };

            return View(model);
        }
    }
}
