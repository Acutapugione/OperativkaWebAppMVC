using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Operativka.Models
{
    public class Report
    {
        public List<ActionsDocument>? Documents { get; set; }

        [Display(Name="Початок періоду")]
        public DateTime? DateTimeBegin { get; set; }

        [Display(Name = "Кінець періоду")]
        public DateTime? DateTimeEnd { get; set; }

        public List<ConsumerCategorie>? ConsumerCategories { get; set; }
        public List<District>? Districts { get; set; }
        public List<PlanningIndicator>? PlanningIndicators { get; set; }
        public List<Settlement>? Settlements { get; set; }

    }
}
