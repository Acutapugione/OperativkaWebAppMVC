using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Operativka.Areas.Identity.Models
{
    public static class Enums
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

        /// <summary>
        /// Gets human-readable version of enum.
        /// </summary>
        /// <returns>effective DisplayAttribute.Name of given enum.</returns>
        public static string GetDisplayName<T>(this T enumValue) where T : IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Argument must be of type Enum");

            DisplayAttribute displayAttribute = enumValue.GetType()
                                                         .GetMember(enumValue.ToString())
                                                         .First()
                                                         .GetCustomAttribute<DisplayAttribute>();

            string displayName = displayAttribute?.GetName();

            return displayName ?? enumValue.ToString();
        }
    }
}