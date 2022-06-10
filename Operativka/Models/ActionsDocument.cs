using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Operativka.Models
{
    public class ActionsDocument
{
        public int Id { get; set; }

        [Display(Name = "Дата")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode =true)]
        public DateTime Date { get; set; }

        [Display(Name = "Населений пункт")]
        public Settlement Settlement { get; set; }

        [Display(Name = "Категорія споживачів")]
        public ConsumerCategorie ConsumerCategorie { get; set; }

        [Display(Name = "Показник планування")]
        public PlanningIndicator PlanningIndicator { get; set; }
        
        public int SettlementId { get; set; }
        public int ConsumerCategorieId { get; set; }
        public int PlanningIndicatorId { get; set; }
        
        [Display(Name ="Заплановано")]
        [Range(1, int.MaxValue)]
        [Required]
        public int Plan_Count { get; set; } = 0;

        [Display(Name = "Виконано")]
        [Range(1, int.MaxValue)]
        [Required]
        public int Fact_Count { get; set; }
        
        [Display(Name ="% виконання")]
        public double ActualPercent
        {
            get
            {
                if (Fact_Count>0 && Plan_Count>0)
                {
                    return Math.Round(Convert.ToDouble( Fact_Count) / Convert.ToDouble(Plan_Count) * 100,2);

                }
                return 0; 
            }
        }
    }
}
