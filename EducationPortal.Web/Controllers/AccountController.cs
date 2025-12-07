using EducationPortal.Web.Data;
using EducationPortal.Web.Helpers;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationPortal.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly EducationContext _context;

        public AccountController(EducationContext context)
        {
            _context = context;
        }

        // LOGIN SAYFASI (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        // LOGIN (POST) - HEM HASH HEM PLAIN DESTEKLİ
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // 1) Kullanıcıyı email ile bul
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                TempData["Error"] = "Geçersiz email veya şifre!";
                return View("~/Views/Account/Login.cshtml");
            }

            // 2) Girilen şifrenin hash'ini hesapla
            var enteredHash = PasswordHelper.Hash(password);

            // 3) İki ihtimal var:
            //    - user.PasswordHash zaten hash'li → enteredHash ile eşit olmalı
            //    - user.PasswordHash eski düz metin → girilen password ile aynı olmalı
            bool isMatch =
                user.PasswordHash == enteredHash ||   // yeni/hash'li kayıt
                user.PasswordHash == password;        // eski/plain kayıt

            if (!isMatch)
            {
                TempData["Error"] = "Geçersiz email veya şifre!";
                return View("~/Views/Account/Login.cshtml");
            }

            // 4) Eğer eski kayıt ise (plain text), şimdi HASH'e çevir ve kaydet
            if (user.PasswordHash == password)
            {
                user.PasswordHash = enteredHash;
                _context.SaveChanges();
            }

            // 5) Cookie'ye claim'leri yaz
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // 6) Rol'e göre yönlendir
            if (user.Role == "Admin")
                return RedirectToAction("Index", "Admin");
            else if (user.Role == "Student")
                return RedirectToAction("Index", "StudentHome");

            return RedirectToAction("Login");
        }

        // LOGOUT
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // KAYIT SAYFASI (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        // KAYIT (POST) - YENİ KULLANICI HER ZAMAN HASH'LENİR
        [HttpPost]
        public IActionResult Register(string fullName, string email, string password)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.Error = "Bu e-posta adresi ile zaten bir hesap var.";
                return View("~/Views/Account/Register.cshtml");
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = PasswordHelper.Hash(password),
                Role = "Student"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }
    }
}
