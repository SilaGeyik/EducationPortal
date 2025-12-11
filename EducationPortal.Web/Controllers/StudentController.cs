using System.Linq;
using System.Threading.Tasks;
using EducationPortal.Web.Data;
using EducationPortal.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // ================== ÖĞRENCİ LİSTESİ (ADMIN) ==================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
           
            var students = await _context.Users
                .Where(u => u.Role == "Student")
                .Select(u => new StudentListViewModel
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    EnrolledCourseCount = _context.Enrollments
                        .Count(e => e.UserId == u.Id)
                })
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return View(students);
        }
    }
}
