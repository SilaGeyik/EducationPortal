using System.Linq;
using System.Security.Claims;
using EducationPortal.Web.Data;
using EducationPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentPanelController : Controller
    {
        private readonly EducationContext _context;

        public StudentPanelController(EducationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1) Oturumdaki Identity Id ve Email
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var emailClaim = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(emailClaim))
                return RedirectToAction("Login", "Account");

            // Identity tarafında Id genelde int -> string'i int'e çeviriyoruz
            if (!int.TryParse(userIdClaim, out var userId))
                return RedirectToAction("Login", "Account");

            // 2) Kullanıcıyı çek
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return RedirectToAction("Login", "Account");

            // 3) Bu öğrencinin kayıt olduğu ders sayısı
            // Enrollment modelinde UserId / User navigation'ını kullanıyoruz
            var totalEnrolled = _context.Enrollments
                .Count(e => e.UserId == userId);

            // 4) Tamamlanan kurslar için henüz alan yoksa 0 bırakıyoruz
            var completedCourses = 0;
            // İleride Enrollment'a IsCompleted eklersek:
            // var completedCourses = _context.Enrollments
            //     .Count(e => e.UserId == userId && e.IsCompleted);

            var vm = new StudentDashboardViewModel
            {
                FullName = user.FullName,
                TotalEnrolledCourses = totalEnrolled,
                CompletedCourses = completedCourses
            };

            return View(vm);
        }
    }
}

