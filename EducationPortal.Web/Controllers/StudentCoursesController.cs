using System;
using System.Linq;
using System.Threading.Tasks;
using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentCoursesController : Controller
    {
        private readonly EducationContext _context;
        private readonly UserManager<User> _userManager;

        public StudentCoursesController(EducationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ================== ÖĞRENCİNİN KAYITLI OLDUĞU KURSLAR ==================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            ViewBag.FullName = currentUser.FullName ?? currentUser.UserName;

            var courses = await _context.Enrollments
                .Where(e => e.UserId == currentUser.Id)             
                .Include(e => e.Course)
                    .ThenInclude(c => c.CourseCategory)
                .Select(e => e.Course)
                .ToListAsync();

            return View(courses);
        }

        // ================== KURSA KAYIT OL (Tüm Kurslar sayfasındaki buton) ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            bool alreadyEnrolled = await _context.Enrollments
                .AnyAsync(e => e.UserId == currentUser.Id && e.CourseId == courseId);

            if (!alreadyEnrolled)
            {
                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    UserId = currentUser.Id
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // ================== KAYITTAN AYRIL (Kayıt Olduğum Kurslar sayfasındaki buton) ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unenroll(int courseId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.UserId == currentUser.Id && e.CourseId == courseId);

            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}

