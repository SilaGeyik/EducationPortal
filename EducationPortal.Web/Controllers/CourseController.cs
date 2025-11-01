using EducationPortal.Web.Models;
using EducationPortal.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _repository;

        public CourseController(ICourseRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var courses = _repository.GetAll();
            return View(courses);
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(Course course)
        {
            if (ModelState.IsValid)
            {
                _repository.Add(course);
                _repository.Save();

                TempData["Success"] = "Kurs başarıyla eklendi!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Kurs eklenirken bir sorun oldu!";
            return View(course);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _repository.GetById(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(course);
                _repository.Save();

                TempData["Success"] = "Kurs başarıyla güncellendi!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Kurs güncellenirken bir hata meydana geldi!";
            return View(course);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var course = _repository.GetById(id);
            if (course == null)
            {
                TempData["Error"] = "Silinmek istenen kurs bulunamadı!";
                return RedirectToAction("Index");
            }

            _repository.Delete(id);
            _repository.Save();

            TempData["Success"] = "Kurs başarıyla silindi!";
            return RedirectToAction("Index");
        }

    }
}
