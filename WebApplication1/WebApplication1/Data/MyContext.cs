using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;

namespace WebApplication1.Data
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public DbSet<TestApp.Models.History>? History { get; set; }

        public DbSet<TestApp.Models.User>? User { get; set; }
    }
}
