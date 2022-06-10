using System.ComponentModel.DataAnnotations;

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
        public DateTime? PlannedDate => ApplicationObjectives.Min(obj => obj.PlannedDate);

        [Display(Name = "Дата виконання")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutionDate => ApplicationObjectives.Max(obj => obj.ExecutionDate);

        [Display(Name = "Виконано")]
        public bool IsExecuted => ApplicationObjectives.All(obj => obj.IsExecuted);

        [Display(Name = "Перелік цілей заявки")]
        public IEnumerable<ApplicationObjective>? ApplicationObjectives { get; set; }

        [Display(Name ="Номер на сайті")]
        public string? OuterAppNum { get; set; }

        [Display(Name = "Сайт")]
        public bool IsFromOuterApp { get; set; } = false;
    }
}
