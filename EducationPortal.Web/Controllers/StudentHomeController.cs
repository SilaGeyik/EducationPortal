using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentHomeController : Controller
    {
        private readonly EducationContext _context;

        public StudentHomeController(EducationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return RedirectToAction("Login", "Account");

            var student = _context.Users
                                  .FirstOrDefault(u => u.Email == email);

            if (student == null)
                return RedirectToAction("Login", "Account");

            var enrollments = _context.Enrollments
                                      .Include(e => e.Course)
                                      .Where(e => e.UserId == student.UserId)
                                      .ToList();

            ViewBag.FullName = student.FullName;
            return View(enrollments);
        }
    }
}
