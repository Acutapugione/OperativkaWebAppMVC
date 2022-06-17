using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static Operativka.Areas.Identity.Models.Enums;

namespace Operativka.Models
{
    public class ApplicationObjective
    {
        public int Id { get; set; }

        [Display(Name = "Тип цілі заявки")]
        public ApplicationObjectiveTypes Type { get; set; }

        [Display(Name = "Дата запланована")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PlannedDate { get; set; } = DateTime.Now;

        [Display(Name = "Дата виконання")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutionDate { get; set; }

        [Display(Name = "Виконано")]
        public bool IsExecuted { get; set; } = false;

        [Display(Name = "Документ")]
        public ApplicationDocument ApplicationDocument { get; set; }
        public int ApplicationDocumentId { get; set; }

    }
}
