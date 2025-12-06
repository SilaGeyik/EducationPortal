using EducationPortal.Web.Data;
using EducationPortal.Web.Helpers;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        // Login Sayfasını Göster (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        // Login İşlemi (POST) - HASH KONTROL
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1) Email'e göre kullanıcıyı bul
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            // 2) Kullanıcı yoksa veya hash uyuşmuyorsa hata
            if (user == null || user.PasswordHash != PasswordHelper.Hash(password))
            {
                TempData["Error"] = "Geçersiz email veya şifre!";
                return View("~/Views/Account/Login.cshtml");
            }

            // 3) Kullanıcı bilgilerini cookie içine ekle
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // 4) Rol kontrolü yap, ona göre yönlendir
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (user.Role == "Student")
            {
                return RedirectToAction("Index", "StudentHome");
            }

            // Bilinmeyen rol olursa tekrar login sayfasına gönder
            return RedirectToAction("Login");
        }

        // Logout Action
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Kayıt Sayfası (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        // Kayıt İşlemi (POST)
        [HttpPost]
        public IActionResult Register(string fullName, string email, string password)
        {
            // Aynı e-posta ile kullanıcı var mı?
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.Error = "Bu e-posta adresi ile zaten bir hesap var.";
                return View("~/Views/Account/Register.cshtml");
            }

            // Yeni kullanıcıyı HASH'LENMİŞ şifre ile oluştur
            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = PasswordHelper.Hash(password),   // 🔒 HASH
                Role = "Student"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }
    }
}
