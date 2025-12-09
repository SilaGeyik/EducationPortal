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
        private readonly EducationContext _courseRepository
            ;

        public CourseController(EducationContext context)
        {
            _courseRepository = context;
        }

       
      
        public IActionResult Index()
        {
            var courses = _courseRepository.Courses
                .Include(c => c.CourseCategory)
                .ToList();

            return View(courses);
        }

       
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories =
                new SelectList(_courseRepository.CourseCategories, "CourseCategoryId", "Name");

            return View();
        }

        
        [HttpPost]
        public IActionResult Add(Course course)
        {
            if (ModelState.IsValid)
            {
                _courseRepository.Courses.Add(course);
                _courseRepository.SaveChanges();

                TempData["Success"] = "Kurs başarıyla eklendi!";
                return RedirectToAction("Index");
            }

            ViewBag.Categories =
                new SelectList(_courseRepository.CourseCategories,
                               "CourseCategoryId",
                               "Name",
                               course.CourseCategoryId);

            TempData["Error"] = "Kurs eklenirken bir hata oluştu!";
            return View(course);
        }

        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _courseRepository.Courses.Find(id);

            if (course == null)
                return NotFound();

            ViewBag.Categories =
                new SelectList(_courseRepository.CourseCategories,
                               "CourseCategoryId",
                               "Name",
                               course.CourseCategoryId);

            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _courseRepository.Courses.Update(course);
                _courseRepository.SaveChanges();

                TempData["Success"] = "Kurs başarıyla güncellendi!";
                return RedirectToAction("Index");
            }

            ViewBag.Categories =
                new SelectList(_courseRepository.CourseCategories,
                               "CourseCategoryId",
                               "Name",
                               course.CourseCategoryId);

            TempData["Error"] = "Kurs güncellenirken bir hata oluştu!";
            return View(course);
        }

        
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var course = _courseRepository.Courses.Find(id);

                if (course == null)
                    return Json(new
                    {
                        success = false,
                        message = "Kurs bulunamadı"
                    });

                _courseRepository.Courses.Remove(course);
                _courseRepository.SaveChanges();

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


        [HttpGet]
        public IActionResult GetCourses()
        {
            try
            {
                var courses = _courseRepository.Courses
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
