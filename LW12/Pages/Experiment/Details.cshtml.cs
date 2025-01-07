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
    public class DetailsModel : PageModel
    {
        private readonly LW12.Data.RazorPagesResearcherContext _context;

        public DetailsModel(LW12.Data.RazorPagesResearcherContext context)
        {
            _context = context;
        }

        public Experiment Experiment { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiment = await _context.Experiment.FirstOrDefaultAsync(m => m.Id == id);

            if (experiment is not null)
            {
                Experiment = experiment;

                return Page();
            }

            return NotFound();
        }
    }
}
