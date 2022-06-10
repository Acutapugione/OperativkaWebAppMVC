using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Operativka.Models
{
    public class ConsumerCategorie
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Коментар")]
        public string? Description { get; set; }

    }
}
