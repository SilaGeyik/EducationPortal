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

        public IActionResult Index()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.User)
                .Include(e => e.Course)
                .OrderByDescending(e => e.EnrollmentDate)
                .ToList();

            // Öğrenci ve ders listelerini ViewBag ile gönderme
            ViewBag.Students = _context.Users.Where(u => u.Role == "Student").ToList();
            ViewBag.Courses = _context.Courses.ToList();

            return View(enrollments);
        }

        [HttpPost]
        public IActionResult Create(int studentId, int courseId)
        {
            try
            {
                // Aynı kaydın olup olmadığını kontrol etme
                var existingEnrollment = _context.Enrollments
                    .FirstOrDefault(e => e.UserId == studentId && e.CourseId == courseId);

                if (existingEnrollment != null)
                {
                    TempData["ErrorMessage"] = "Bu öğrenci zaten bu derse kayıtlı!";
                    return RedirectToAction("Index");
                }

                var enrollment = new Enrollment
                {
                    UserId = studentId,
                    CourseId = courseId,
                    EnrollmentDate = DateTime.Now
                };

                _context.Enrollments.Add(enrollment);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Kayıt başarıyla eklendi!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kayıt eklenirken hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var enrollment = _context.Enrollments
                    .Include(e => e.User)
                    .Include(e => e.Course)
                    .FirstOrDefault(e => e.EnrollmentId == id);

                if (enrollment != null)
                {
                    _context.Enrollments.Remove(enrollment);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = $"'{enrollment.User.FullName}' öğrencisinin '{enrollment.Course.Title}' dersindeki kaydı silindi!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Kayıt bulunamadı!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Kayıt silinirken hata oluştu: " + ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}