using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        // /Student  veya /Student/Index
        public IActionResult Index()
        {
            var students = _context.Users
                .Where(u => u.Role == "Student")
                .ToList();

            return View(students);   // Views/Student/Index.cshtml
        }
    }
}
