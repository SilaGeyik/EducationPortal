using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class StudentController : Controller
    {
        private readonly EducationContext _context;

        public StudentController(EducationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Veritabanındaki tüm öğrencileri çek
            var students = _context.Users
                .Where(u => u.Role == "Student") // Eğer User tablon öğrencileri tutuyorsa
                .ToList();

            ViewData["Title"] = "Öğrenciler";

            // ✅ View'e model olarak öğrencileri gönder
            return View(students);
        }
    }
}
