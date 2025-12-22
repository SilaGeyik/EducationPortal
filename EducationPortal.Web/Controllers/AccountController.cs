using System.Linq;
using System.Threading.Tasks;
using EducationPortal.Web.Hubs;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace EducationPortal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IHubContext<NotificationHub> _hub;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole<int>> roleManager,
            IHubContext<NotificationHub> hub)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _hub = hub;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "E-posta ve şifre zorunludur.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                userName: email,
                password: password,
                isPersistent: rememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    if (user.Role == "Admin")
                        return RedirectToAction("Index", "Admin");

                    if (user.Role == "Student")
                        return RedirectToAction("Index", "StudentPanel");
                }

                return RedirectToAction("Index", "StudentPanel");
            }

            TempData["Error"] = "Geçersiz e-posta veya şifre.";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Ad Soyad, e-posta ve şifre zorunludur.";
                return View();
            }

            var existing = await _userManager.FindByEmailAsync(email);
            if (existing != null)
            {
                ViewBag.Error = "Bu e-posta ile daha önce kayıt yapılmış.";
                return View();
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                UserName = email,
                Role = "Student"
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
              
                if (!await _roleManager.RoleExistsAsync("Student"))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>("Student"));
                }

                
                await _userManager.AddToRoleAsync(user, "Student");

               
                var totalStudents = _userManager.Users.Count(u => u.Role == "Student");

               
                await _hub.Clients.All.SendAsync(
                    "ReceiveNotification",
                    $"Toplam {totalStudents} öğrenci kayıt oldu."
                );

                ViewBag.Success = "Kayıt başarılı! Giriş yapabilirsiniz.";
                ModelState.Clear();
                return View();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
