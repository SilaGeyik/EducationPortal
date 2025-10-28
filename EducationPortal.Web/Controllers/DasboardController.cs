using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserName")))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewData["Title"] = "Dashboard";
            return View();
        }
    }
}
