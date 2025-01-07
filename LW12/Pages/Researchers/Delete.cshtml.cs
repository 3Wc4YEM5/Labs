using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LW12.Data;
using LW12.Models;

namespace LW12.Pages_Movies
{
    public class DeleteModel : PageModel
    {
        private readonly LW12.Data.RazorPagesResearcherContext _context;

        public DeleteModel(LW12.Data.RazorPagesResearcherContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Researcher Researcher { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researcher = await _context.Researcher.FirstOrDefaultAsync(m => m.ResearcherId == id);

            if (researcher is not null)
            {
                Researcher = researcher;

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

            var researcher = await _context.Researcher.FindAsync(id);
            if (researcher != null)
            {
                Researcher = researcher;
                _context.Researcher.Remove(Researcher);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
