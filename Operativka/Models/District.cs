using System.ComponentModel.DataAnnotations;

namespace Operativka.Models
{
    public class District
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Код організації")]
        public int OrgId { get; set; }

        public IEnumerable<Settlement>? Settlements { get; set; }
        public IEnumerable<Consumer>? Consumers { get; set; }
    }
}
