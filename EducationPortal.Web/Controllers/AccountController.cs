using EducationPortal.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

            if (user == null) {

                if (user.Role == "Admin")
                    return RedirectToAction("Index", "Admin");
                else
                    return RedirectToAction("Index", "Student");
            }
            ViewBag.Error = "Geçersiz e-posta veya şifresi";
            return View();
        }
    }
}
