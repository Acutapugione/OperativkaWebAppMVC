using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Operativka.Models
{
    public class Settlement
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Дільниця")]
        public District District { get; set; }

        public int DistrictId { get; set; }
    }
}
