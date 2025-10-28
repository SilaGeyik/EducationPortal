using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly EducationContext _context;

        public CourseController(EducationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();//veritabanındaki tüm dersleri çeker
            return View(courses); //view liste gönderme
        }


    
    [HttpGet]
        public IActionResult Add()
        {
            ViewData["Title"] = "Yeni Ders Ekle";
            return View();
        }

        [HttpPost]
        public IActionResult Add(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();

                // Başarılı ekleme sonrası listeye dön
                return RedirectToAction("Index");
            }

            // Model hatalıysa aynı sayfada kal
            return View(course);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Ders Sil";
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Ders Düzenle";
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(course);
        }



    }

}