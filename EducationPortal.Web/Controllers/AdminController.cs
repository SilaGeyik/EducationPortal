using EducationPortal.Web.Data;
using EducationPortal.Web.Helpers;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly EducationContext _context;

        public AdminController(EducationContext context)
        {
            _context = context;
        }

        //Admin Ana Sayfa 
        public IActionResult Index()
        {
            // İstatistik verileri
            ViewBag.StudentCount = _context.Users.Count(u => u.Role == "Student");
            ViewBag.CourseCount = _context.Courses.Count();
            ViewBag.EnrollmentCount = _context.Enrollments.Count();
            return View("~/Views/Admin/Index.cshtml");
        }

        //Öğrenci Listesi
        public IActionResult Students()
        {
            var students = _context.Users
                .Where(u => u.Role == "Student")
                .ToList();
            return View("~/Views/Admin/Students.cshtml", students);
        }

        //Yeni Öğrenci Ekleme (GET)
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View("~/Views/Admin/AddStudent.cshtml");
        }

        //Yeni Öğrenci Ekleme (POST) 
        [HttpPost]
        public IActionResult AddStudent(string fullName, string email, string password)
        {
            if (ModelState.IsValid)
            {
                var newStudent = new Models.User
                {
                    FullName = fullName,
                    Email = email,
                    Password = password,
                    Role = "Student"
                };

                _context.Users.Add(newStudent);
                _context.SaveChanges();
                return RedirectToAction("Students");
            }
            return View("~/Views/Admin/AddStudent.cshtml");
        }

        // AJAX için öğrenci ekleme
        // AJAX ile öğrenci ekleme
        [HttpPost]
        public JsonResult AddStudentAjax(string fullName, string email, string password)
        {
            // Aynı mailden var mı kontrol et
            var existing = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existing != null)
            {
                return Json(new
                {
                    success = false,
                    message = "Bu e-posta ile kayıtlı bir öğrenci zaten var."
                });
            }

            var student = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = PasswordHelper.Hash(password), // 🔒 HASH
                Role = "Student"
            };

            _context.Users.Add(student);
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Öğrenci başarıyla eklendi.",
                student = new
                {
                    userId = student.UserId,
                    fullName = student.FullName,
                    email = student.Email,
                    role = student.Role
                }
            });
        }

        // AJAX ile öğrenci silme
        [HttpPost]
        public JsonResult DeleteStudentAjax(int id)
        {
            var student = _context.Users.FirstOrDefault(u => u.UserId == id && u.Role == "Student");
            if (student == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Öğrenci bulunamadı."
                });
            }

            _context.Users.Remove(student);
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Öğrenci başarıyla silindi."
            });
        }
    }
}