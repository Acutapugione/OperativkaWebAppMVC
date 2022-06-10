using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Operativka.Models
{
    public class ActionsDocumentViewModel
    {
        //public List<ActionsDocument>? Documents { get; set; }
        public PaginatedList<ActionsDocument>? Documents { get; set; }
        
        public SelectList?  Districts{ get; set; }
        public SelectList?  Settlements{ get; set; }
        public SelectList?  PlanningIndicators { get; set; }
        public SelectList?  ConsumerCategories { get; set; }

        [Display(Name = "Дільниця")]
        public string? SelectedDistrict { get; set; }
        [Display(Name = "Населений пункт")]
        public string? SelectedSettlement { get; set; }
        [Display(Name = "Показник планування")]
        public string? SelectedPlanningIndicator { get; set; }
        [Display(Name = "Категорія споживачів")]
        public string? SelectedConsumerCategorie { get; set; }

        public int? pageNumber { get; set; }
        public Dictionary<string, string>? CurrentFilter { get; set; }

        public DateTime? DateTimeBegin { get; set; }
        public DateTime? DateTimeEnd { get; set; }
    }
}
