using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
