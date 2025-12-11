using System.Linq;
using System.Threading.Tasks;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EducationPortal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            // Identity ile giriş denemesi
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
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "E-posta ve şifre zorunludur.");
                return View();
            }

            // Aynı e-posta var mı?
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
                // Student rolü yoksa oluştur
                if (!await _roleManager.RoleExistsAsync("Student"))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>("Student"));
                }

                // Rol ekle
                await _userManager.AddToRoleAsync(user, "Student");

                // OTOMATİK LOGIN YOK
                
                ViewBag.Success = "Kayıt işlemi başarılı! Giriş yapabilirsiniz.";

                // Form alanlarını boşalt
                ModelState.Clear();

                return View();   
            }

            // Hataları göster
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }


        // =====================================================
        // LOGOUT
        // =====================================================
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // =====================================================
        // ACCESS DENIED
        // =====================================================
        public IActionResult AccessDenied()
        {
            return View(); // Views/Account/AccessDenied.cshtml
        }
    }
}
