using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Operativka.Models
{
    public class ApplicationDocument
    {
        public int Id { get; set; }
        
        [Display(Name ="Споживач")]
        public Consumer Consumer { get; set; }
        public int ConsumerId { get; set; }

        [Display(Name = "Дата запланована")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PlannedDate => (ApplicationObjectives is null || ApplicationObjectives.Count == 0) ? DateTime.Now : ApplicationObjectives.Min(obj => obj.PlannedDate);

        [Display(Name = "Дата виконання")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutionDate => (ApplicationObjectives is null || ApplicationObjectives.Count == 0) ? DateTime.Now : ApplicationObjectives.Max(obj => obj.ExecutionDate);

        [Display(Name = "Виконано")]
        public bool IsExecuted => (ApplicationObjectives is null || ApplicationObjectives.Count==0)? false: ApplicationObjectives.All(obj => obj.IsExecuted);

        [Display(Name = "Перелік цілей заявки")]
        public List<ApplicationObjective>? ApplicationObjectives { get; set; } = new List<ApplicationObjective>();

        [Display(Name = "Номер на сайті")]
        public string? OuterAppNum { get; set; }

        [Display(Name = "Сайт")]
        public bool IsFromOuterApp { get; set; } = false;
    }
}
