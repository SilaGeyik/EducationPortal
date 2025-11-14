using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class DashboardController: Controller
    {
        private readonly EducationContext _context;

        public DashboardController(EducationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.StudentCount = _context.Users.Count(u=> u.Role =="Student");
            ViewBag.CourseCount = _context.Courses.Count();
            ViewBag.EnrollmentCount = _context.Enrollments.Count();


            return View();
        }
    }
}