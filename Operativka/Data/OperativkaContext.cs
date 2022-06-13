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

        public DbSet<Operativka.Models.Consumer>? Consumers { get; set; }

        public DbSet<Operativka.Models.ApplicationDocument>? ApplicationDocuments { get; set; }

        public DbSet<Operativka.Models.ApplicationObjective>? ApplicationObjectives { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Consumer>().ToTable(nameof(Consumers));
            builder.Entity<ApplicationObjective>().ToTable(nameof(ApplicationObjectives));
            builder.Entity<ApplicationDocument>().ToTable(nameof(ApplicationDocuments));

            builder.Entity<ActionsDocument>().ToTable(nameof(ActionsDocuments));
            builder.Entity<PlanningIndicator>().ToTable(nameof(PlanningIndicators));
            builder.Entity<ConsumerCategorie>().ToTable(nameof(ConsumerCategories));

            //modelBuilder.Entity<Settlement>().Ignore(x => x.District);
            builder.Entity<Settlement>().ToTable(nameof(Settlements));

            builder.Entity<District>()
                .Property(x => x.Id).ValueGeneratedNever();
            builder.Entity<District>().ToTable(nameof(Districts));
            //base.OnModelCreating(modelBuilder);
        }
    }
}
