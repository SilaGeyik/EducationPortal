using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Web.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly EducationContext _context;
        public EnrollmentController(EducationContext context)
        {
            _context = context;
        }
        //tüm kayıtları listele 
        public IActionResult Index()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.User) //ınclude ile iki tabloyu birden çektim
                .Include(e => e.Course)
                .ToList();

            return View(enrollments);
        }
    }
}
