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
            // Kurs dropdown
            ViewBag.Courses = await _context.Courses
                .OrderBy(c => c.Title)
                .Select(c => new CourseDropdownItem
                {
                    CourseId = c.CourseId,
                    Title = c.Title
                })
                .ToListAsync();

            // Tüm öğrenciler (başlangıç)
            var students = await _context.Users
                .Where(u => u.Role == "Student")
                .Select(u => new StudentListViewModel
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    EnrolledCourseCount = _context.Enrollments.Count(e => e.UserId == u.Id)
                })
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return View(students);
        }

        // ================== KURSA GÖRE FİLTRE (AJAX) ==================
        // courseId = 0 gelirse "Hepsi"
        [HttpGet]
        public async Task<IActionResult> FilterByCourse(int courseId)
        {
            // courseId 0 => hepsi
            if (courseId == 0)
            {
                var allStudents = await _context.Users
                    .Where(u => u.Role == "Student")
                    .Select(u => new StudentListViewModel
                    {
                        UserId = u.Id,
                        FullName = u.FullName,
                        Email = u.Email,
                        EnrolledCourseCount = _context.Enrollments.Count(e => e.UserId == u.Id)
                    })
                    .OrderBy(s => s.FullName)
                    .ToListAsync();

                return PartialView("_StudentTable", allStudents);
            }

            // Seçilen kursa kayıtlı öğrenciler
            var studentIds = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Select(e => e.UserId)
                .Distinct()
                .ToListAsync();

            var filteredStudents = await _context.Users
                .Where(u => u.Role == "Student" && studentIds.Contains(u.Id))
                .Select(u => new StudentListViewModel
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    EnrolledCourseCount = _context.Enrollments.Count(e => e.UserId == u.Id)
                })
                .OrderBy(s => s.FullName)
                .ToListAsync();

            return PartialView("_StudentTable", filteredStudents);
        }
    }

    // Dropdown için küçük DTO (ViewModel değil, controller içi yardımcı)
    public class CourseDropdownItem
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
    }
}
