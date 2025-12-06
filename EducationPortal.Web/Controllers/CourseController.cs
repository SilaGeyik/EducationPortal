using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller
    {
        private readonly EducationContext _context;

        public CourseController(EducationContext context)
        {
            _context = context;
        }

        // ================================
        // LIST
        // ================================
        public IActionResult Index()
        {
            var courses = _context.Courses
                .Include(c => c.CourseCategory)
                .ToList();

            return View(courses);
        }

        // ================================
        // ADD - GET
        // ================================
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories =
                new SelectList(_context.CourseCategories, "CourseCategoryId", "Name");

            return View();
        }

        // ================================
        // ADD - POST
        // ================================
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

            ViewBag.Categories =
                new SelectList(_context.CourseCategories,
                               "CourseCategoryId",
                               "Name",
                               course.CourseCategoryId);

            TempData["Error"] = "Kurs eklenirken bir hata oluştu!";
            return View(course);
        }

        // ================================
        // EDIT - GET
        // ================================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _context.Courses.Find(id);

            if (course == null)
                return NotFound();

            ViewBag.Categories =
                new SelectList(_context.CourseCategories,
                               "CourseCategoryId",
                               "Name",
                               course.CourseCategoryId);

            return View(course);
        }

        // ================================
        // EDIT - POST
        // ================================
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

            ViewBag.Categories =
                new SelectList(_context.CourseCategories,
                               "CourseCategoryId",
                               "Name",
                               course.CourseCategoryId);

            TempData["Error"] = "Kurs güncellenirken bir hata oluştu!";
            return View(course);
        }

        // ================================
        // DELETE - AJAX
        // ================================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var course = _context.Courses.Find(id);

                if (course == null)
                    return Json(new
                    {
                        success = false,
                        message = "Kurs bulunamadı"
                    });

                _context.Courses.Remove(course);
                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Kurs başarıyla silindi"
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Silme işlemi hatalı: " + ex.Message
                });
            }
        }


        // ================================
        // GET COURSES - AJAX
        // ================================
        [HttpGet]
        public IActionResult GetCourses()
        {
            try
            {
                var courses = _context.Courses
                    .Include(c => c.CourseCategory)
                    .Select(c => new
                    {
                        c.CourseId,
                        c.Title,
                        c.Credits,
                        c.Instructor,
                        CategoryName = c.CourseCategory.Name
                    })
                    .ToList();

                return Json(new
                {
                    success = true,
                    data = courses
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Liste yüklenirken hata oluştu: " + ex.Message
                });
            }
        }
    }
}
