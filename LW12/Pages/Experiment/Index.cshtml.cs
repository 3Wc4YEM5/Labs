using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LW12.Data;
using LW12.Models;

namespace LW12.Pages_Experiment
{
    public class IndexModel : PageModel
    {
        private readonly LW12.Data.RazorPagesResearcherContext _context;

        public IndexModel(LW12.Data.RazorPagesResearcherContext context)
        {
            _context = context;
        }

        public IList<Experiment> Experiment { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Experiment = await _context.Experiment.ToListAsync();
        }
    }
}
