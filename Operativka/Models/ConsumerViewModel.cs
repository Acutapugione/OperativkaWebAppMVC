using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Operativka.Models
{
    public class ConsumerViewModel
    {
        public PaginatedList<Consumer>? Consumers { get; set; }

        public SelectList? Districts { get; set; }

        [Display(Name = "Дільниця")]
        public string? SelectedDistrict { get; set; }

        [Display(Name = "Особовий рахунок")]
        public string? InsertedPersonalAccountCode { get; set; }

        public int? pageNumber { get; set; }
        public Dictionary<string, string>? CurrentFilter { get; set; }
    }
}
