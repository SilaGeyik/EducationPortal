using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly EducationContext _context;

        public AdminController(EducationContext context)
        {
            _context = context;
        }

        //  öğrenci listesi
        public IActionResult Students()
        {
            var students = _context.Users.Where(u => u.Role == "Student").ToList();
            return View();
        }

        //yeni öğrenci ekleme sayfası(GET)
        [HttpGet]
        public IActionResult AddStudent() {

            return View();
        }

        //yeni öğrenci ekleme(POST)
        [HttpPost]
        public IActionResult AddStudent(string fullName, string email, string password)
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
    } 
}
