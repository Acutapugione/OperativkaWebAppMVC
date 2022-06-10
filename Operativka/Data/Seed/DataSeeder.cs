using Operativka.Models;

namespace Operativka.Data.Seed
{
    public static class DataSeeder
    {
        public static void ExecuteSeed(OperativkaContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!context.Districts.Any())
            {
                HashSet<District> districts = new()
                {
                    new District { Id = 2, Name = "Білозерська дільниця", OrgId = 7200 },
                    new District { Id = 3, Name = "Н.Каховська дільниця", OrgId = 5100 },
                    new District { Id = 4, Name = "В.Олександрівське МРУ", OrgId = 2100 },
                    new District { Id = 5, Name = "Високопільська дільниця", OrgId = 2200 },
                    new District { Id = 6, Name = "Бериславська дільниця", OrgId = 5300 },
                    new District { Id = 7, Name = "Чаплинська дільниця", OrgId = 4300 },
                    new District { Id = 8, Name = "Генічеське МРУ", OrgId = 3000 },
                    new District { Id = 9, Name = "Голопристанська дільниця", OrgId = 6200 },
                    new District { Id = 10, Name = "Каховська дільниця", OrgId = 5200 },
                    new District { Id = 11, Name = "Каланчакська дільниця", OrgId = 4100 },
                    new District { Id = 12, Name = "Скадовська дільниця", OrgId = 4200 },
                    new District { Id = 13, Name = "Олешківська дільниця", OrgId = 6100 },
                    new District { Id = 14, Name = "Верхньорогачицька дільниця", OrgId = 0 },
                    new District { Id = 20, Name = "Херсонська дільниця", OrgId = 7100 },
                };
                context.Districts.AddRange(districts);

                context.SaveChanges();
            }
        }
    }
}
