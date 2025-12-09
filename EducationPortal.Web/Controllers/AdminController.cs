using EducationPortal.Web.Data;
using EducationPortal.Web.Hubs;
using EducationPortal.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace EducationPortal.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly EducationContext _context;
        private readonly IHubContext<NotificationHub> _notificationHub;

        
        public AdminController(EducationContext context, IHubContext<NotificationHub> notificationHub)
        {
            _context = context;
            _notificationHub = notificationHub; // signalR injection
        }

        public IActionResult Index()
        {
            return View();
        }

        // ------------------------
        // Öğrenci listesi
        // ------------------------
        public IActionResult Students()
        {
            var students = _context.Users
                .Where(x => x.Role == "Student") 
                .ToList();

            return View(students);
        }


     
        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }


       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Normal kayıt işlemi -------------------------------------
            _context.Users.Add(model);
            await _context.SaveChangesAsync();
            

            var fullName = $"{model.FullName}";


            await _notificationHub.Clients.All.SendAsync(
                "ReceiveNotification",
                $"{fullName} isimli öğrenci sisteme eklendi."
            );


            
            return RedirectToAction(nameof(Students));
        }


        // ---------------------------------------------------
        // Öğrenci silme
        // ---------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = _context.Users.Find(id);
            if (student == null)
                return NotFound();

            _context.Users.Remove(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Students));
        }
    }
}
