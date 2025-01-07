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
    public class DeleteModel : PageModel
    {
        private readonly LW12.Data.RazorPagesResearcherContext _context;

        public DeleteModel(LW12.Data.RazorPagesResearcherContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var experiment = await _context.Experiment.FindAsync(id);
            if (experiment != null)
            {
                Experiment = experiment;
                _context.Experiment.Remove(Experiment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
