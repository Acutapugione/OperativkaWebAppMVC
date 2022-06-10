using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static Operativka.Areas.Identity.Models.Enums;

namespace Operativka.Models
{
    public class ApplicationObjective
    {
        public int Id { get; set; }

        [Display(Name = "Ціль заявки")]
        public ApplicationObjectiveTypes Type { get; set; }

        [Display(Name = "Дата запланована")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PlannedDate { get; set; }

        [Display(Name = "Дата виконання")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutionDate { get; set; }

        public ApplicationDocument ApplicationDocument { get; set; }
        public int ApplicationDocumentId { get; set; }
    }
}
