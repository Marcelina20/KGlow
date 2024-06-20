using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KGlow.Models;

namespace KGlow.Data
{
    public class KGlowContext : DbContext
    {
        public KGlowContext (DbContextOptions<KGlowContext> options)
            : base(options)
        {
        }

        public DbSet<KGlow.Models.Product> Product { get; set; } = default!;

        public DbSet<KGlow.Models.Brand>? Brand { get; set; }

        public DbSet<KGlow.Models.Category>? Category { get; set; }
    }
}
