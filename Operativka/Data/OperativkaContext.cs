using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Operativka.Models;

namespace Operativka.Data
{
    public class OperativkaContext : DbContext
    {
        public OperativkaContext (DbContextOptions<OperativkaContext> options)
            : base(options)
        {
        }

        public DbSet<Operativka.Models.District>? Districts { get; set; }

        public DbSet<Operativka.Models.Settlement>? Settlements { get; set; }

        public DbSet<Operativka.Models.ConsumerCategorie>? ConsumerCategories { get; set; }

        public DbSet<Operativka.Models.PlanningIndicator>? PlanningIndicators { get; set; }

        public DbSet<Operativka.Models.ActionsDocument>? ActionsDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<ActionsDocument>().ToTable(nameof(ActionsDocuments));
            builder.Entity<PlanningIndicator>().ToTable(nameof(PlanningIndicators));
            builder.Entity<ConsumerCategorie>().ToTable(nameof(ConsumerCategories));

            //modelBuilder.Entity<Settlement>().Ignore(x => x.District);
            builder.Entity<Settlement>().ToTable(nameof(Settlements));

            builder.Entity<District>()
                .Property(x=>x.Id).ValueGeneratedNever();
            builder.Entity<District>().ToTable(nameof(Districts));
            //base.OnModelCreating(modelBuilder);
        }
    }
}
