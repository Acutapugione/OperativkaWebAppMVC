using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Xml.Linq;

namespace Operativka.Models
{
    public class Consumer
    {
        public int Id { get; set; }

        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Display(Name = "По-батькові")]
        public string Patronymic { get; set; }

        [Display(Name = "ПІБ")]
        public string FullName => $"{LastName} {FirstName} {Patronymic}";
       
        [Display(Name = "Адреса")]
        public string? Address { get; set; }

        [Display(Name = "Номер телефону")]
        [Phone]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Особовий рахунок")]
        public int? PersonalAccountCode { get; set; }

        [Display(Name = "Дільниця")]
        public District District { get; set; }
        public int DistrictId { get; set; }
    }
}
