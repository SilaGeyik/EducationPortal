using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class CourseController : Controller
    {
        private readonly EducationContext _context;

        public CourseController(EducationContext context)
        {
            _context = context;
        }

        // LIST
        public IActionResult Index()
        {
            var courses = _context.Courses
                .Include(c => c.CourseCategory)
                .ToList();

            return View(courses);
        }

        // ADD PAGE (GET)
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_context.CourseCategories, "CourseCategoryId", "Name");
            return View();
        }

        // ADD (POST)
        [HttpPost]
        public IActionResult Add(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();
                TempData["Success"] = "Kurs başarıyla eklendi!";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.CourseCategories, "CourseCategoryId", "Name", course.CourseCategoryId);
            TempData["Error"] = "Kurs eklenirken bir hata oluştu!";
            return View(course);
        }

        // EDIT PAGE (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.CourseCategories, "CourseCategoryId", "Name", course.CourseCategoryId);
            return View(course);
        }

        // EDIT (POST)
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
                TempData["Success"] = "Kurs başarıyla güncellendi!";
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.CourseCategories, "CourseCategoryId", "Name", course.CourseCategoryId);
            TempData["Error"] = "Kurs güncellenirken bir hata oluştu!";
            return View(course);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
                return NotFound();

            _context.Courses.Remove(course);
            _context.SaveChanges();

            TempData["Success"] = "Kurs silindi!";
            return RedirectToAction("Index");
        }
    }
}


