using System.ComponentModel.DataAnnotations;

namespace Operativka.Models
{
    public class ApplicationDocument
    {
        public int Id { get; set; }
        
        public Consumer Consumer { get; set; }
        public int ConsumerId { get; set; }

        [Display(Name = "Дата запланована")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PlannedDate { get;}

        [Display(Name = "Дата виконання")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExecutionDate { get;}
        public bool IsExecuted { get; }

        public IEnumerable<ApplicationObjective>? ApplicationObjectives { get; set; }

    }
}
