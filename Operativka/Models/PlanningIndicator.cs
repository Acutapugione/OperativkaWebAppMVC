using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Operativka.Models
{
    public class PlanningIndicator
    {
        public int Id { get; set; }

        [Display(Name = "Назва")]
        public string Name { get; set; }

        [Display(Name = "Назва виконання")]
        public string Name_Fact { get; set; }

        public static string Plan_Indicator => "План";

        public static string Fact_Indocator => "Факт";

    }
}
