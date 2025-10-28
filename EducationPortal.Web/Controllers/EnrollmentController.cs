using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly EducationContext _context;

        public EnrollmentController(EducationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var enrollments = _context.Enrollments.ToList();//veritabanındaki tüm dersleri çeker
            return View(enrollments); //view liste gönderme
        }
    }
}