using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly EducationContext _context;

        public AccountController(EducationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // ✅ Session’a kullanıcı bilgilerini kaydet
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Role);

                // ✅ Dashboard’a yönlendir
                return RedirectToAction("Index", "Dashboard");
            }

            // ❌ Hatalıysa
            ViewBag.Error = "E-posta veya şifre hatalı!";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}

