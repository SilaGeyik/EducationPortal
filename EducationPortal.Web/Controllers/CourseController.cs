using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
