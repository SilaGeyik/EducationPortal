using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using EducationPortal.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.Web.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly EducationContext _context;
        private readonly UserManager<User> _userManager;

        public CourseController(
            ICourseRepository courseRepository,
            EducationContext context,
            UserManager<User> userManager)
        {
            _courseRepository = courseRepository;
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Kategori dropdown’unu doldurur.
        /// selectedCategoryId verilirse, o kategori seçili gelir.
        /// </summary>
        private void FillCategoriesDropDown(int? selectedCategoryId = null)
        {
            var categories = _context.CourseCategories
                                     .AsNoTracking()
                                     .ToList();

            ViewBag.CategoryList = new SelectList(
                categories,
                "CourseCategoryId",   // value
                "Name",              // text
                selectedCategoryId   // seçili olan
            );
        }

        // LISTE (Admin + Student)
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> Index()
        {
            var courses = _courseRepository.GetAll(); // Course + Category

            var enrolledIds = new List<int>();

            if (User.IsInRole("Student"))
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    enrolledIds = await _context.Enrollments
                        .Where(e => e.UserId == currentUser.Id)
                        .Select(e => e.CourseId)
                        .ToListAsync();
                }
            }

            ViewBag.EnrolledCourseIds = enrolledIds;

            return View(courses);
        }

        // DETAY
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Details(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // YENİ KURS EKLE
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            FillCategoriesDropDown(); // dropdown dolu gelsin
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Course model)
        {
            // Seçilen kategori gerçekten var mı?
            bool categoryExists = _context.CourseCategories
                .Any(c => c.CourseCategoryId == model.CourseCategoryId);

            if (!categoryExists)
            {
                ModelState.AddModelError("CourseCategoryId", "Lütfen geçerli bir kategori seçiniz.");
            }

            if (!ModelState.IsValid)
            {
                FillCategoriesDropDown(model.CourseCategoryId);
                return View(model);
            }

            _courseRepository.Add(model);
            return RedirectToAction(nameof(Index));
        }

        // DÜZENLE
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null)
                return NotFound();

            // Mevcut kategorisi seçili gelsin
            FillCategoriesDropDown(course.CourseCategoryId);

            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course model)
        {
            // Seçilen kategori gerçekten var mı?
            bool categoryExists = _context.CourseCategories
                .Any(c => c.CourseCategoryId == model.CourseCategoryId);

            if (!categoryExists)
            {
                ModelState.AddModelError("CourseCategoryId", "Lütfen geçerli bir kategori seçiniz.");
            }

            if (!ModelState.IsValid)
            {
                FillCategoriesDropDown(model.CourseCategoryId);
                return View(model);
            }

            _courseRepository.Update(model);
            return RedirectToAction(nameof(Index));
        }

        // SİL
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null)
                return NotFound();

            _courseRepository.Delete(id);
            return Json(new { success = true });
        }
    }
}
