using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LW12.Data;
using LW12.Models;

namespace LW12.Pages_Projects
{
    public class CreateModel : PageModel
    {
        private readonly LW12.Data.RazorPagesResearcherContext _context;

        public CreateModel(LW12.Data.RazorPagesResearcherContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Projects Projects { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Projects.Add(Projects);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
