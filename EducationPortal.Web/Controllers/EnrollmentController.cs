using System.Linq;
using System.Threading.Tasks;
using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnrollmentController : Controller
    {
        private readonly EducationContext _context;

        public EnrollmentController(EducationContext context)
        {
            _context = context;
        }

        // ================== TÜM KAYITLARIN LİSTESİ (ADMIN) ==================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                    .ThenInclude(c => c.CourseCategory)
                .Include(e => e.User)
                .OrderBy(e => e.Course.Title)
                .ThenBy(e => e.User.FullName)
                .ToListAsync();

            return View(enrollments);
        }

        // ================== KAYDI SİL (ADMIN) ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
