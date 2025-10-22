using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly EducationContext _context;

        public CourseController(EducationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();//veritabanındaki tüm dersleri çeker
            return View(courses); //view liste gönderme
        }
    }
}
