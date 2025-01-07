using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LW12.Models;

namespace LW12.Data
{
    public class RazorPagesResearcherContext : DbContext
    {
        public RazorPagesResearcherContext (DbContextOptions<RazorPagesResearcherContext> options)
            : base(options)
        {
        }

        public DbSet<LW12.Models.Researcher> Researcher { get; set; } = default!;
        public DbSet<LW12.Models.Projects> Projects { get; set; } = default!;
        public DbSet<LW12.Models.Experiment> Experiment { get; set; } = default!;
    }
}
