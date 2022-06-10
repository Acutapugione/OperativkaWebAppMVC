using System.ComponentModel.DataAnnotations;

namespace Operativka.Areas.Identity.Models
{
    public class Enums
    {
        public enum Roles
        {
            SuperAdmin,
            Admin,
            Moderator,
            Basic
        }

        public enum ApplicationObjectiveTypes
        {
            [Display(Name ="Акт обстеження")]
            SurveyReport,
            [Display(Name = "Для отримання довідки")]
            ConsumerHelp,
            [Display(Name = "Переопломбування ПЛГ")]
            GasMeterDeviceResealing,
            [Display(Name = "Зняття показників ПЛГ")]
            GasMeterDeviceLogging,
            [Display(Name = "Переоформлення особового рахунку")]
            PersonalAccountCodeRenewal,
            [Display(Name = "Зняття ПЛГ на перевірку")]
            GasMeterDeviceWithdrawalInspection
        }
    }
}