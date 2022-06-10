using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Operativka.Areas.Identity.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Display(Name ="Ім'я")]
        public string FirstName { get; set; }
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
    }
}
