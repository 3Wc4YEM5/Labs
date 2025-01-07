using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LW12.Data;
using LW12.Models;

namespace LW12.Pages_Movies
{
    public class EditModel : PageModel
    {
        private readonly LW12.Data.RazorPagesResearcherContext _context;

        public EditModel(LW12.Data.RazorPagesResearcherContext context)
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

            var researcher =  await _context.Researcher.FirstOrDefaultAsync(m => m.ResearcherId == id);
            if (researcher == null)
            {
                return NotFound();
            }
            Researcher = researcher;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Researcher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResearcherExists(Researcher.ResearcherId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ResearcherExists(int id)
        {
            return _context.Researcher.Any(e => e.ResearcherId == id);
        }
    }
}
