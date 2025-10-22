using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly EducationContext _context;

        public StudentController(EducationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var students = _context.Users
                                   .Where(u=> u.Role =="student")
                                   .ToList();
            return View(students);
        }
    }
}
