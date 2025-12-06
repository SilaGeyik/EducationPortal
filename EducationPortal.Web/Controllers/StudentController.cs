using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller 
    {
        private readonly EducationContext _context;

        public StudentController(EducationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Veritabanındaki tüm öğrencileri çekme
            var students = _context.Users
                .Where(u => u.Role == "Student")
                .ToList();

            ViewData["Title"] = "Öğrenciler";
            return View(students);
        }
    }
}