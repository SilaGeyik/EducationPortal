using Microsoft.AspNetCore.Identity; 
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationPortal.Web.Models
{
    
    public class User : IdentityUser<int>
    {
        
        public string FullName { get; set; } = null!;

        
        public string Role { get; set; } = null!;

        
        [NotMapped]
        public string? Password { get; set; }
    }
}