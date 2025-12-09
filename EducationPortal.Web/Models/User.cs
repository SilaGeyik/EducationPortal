using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Web.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        // Formdan gelen düz şifre – veritabanında KOLONU YOK
        [NotMapped]
        public string? Password { get; set; }

        // Veritabanındaki gerçek kolon
        public string PasswordHash { get; set; } = null!; //Hash'li saklayacağım için şifreleri

        public string Role { get; set; } = null!;
    }
}
