using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LW12.Data;
using LW12.Models;

namespace LW12.Pages_Projects
{
    public class DetailsModel : PageModel
    {
        private readonly LW12.Data.RazorPagesResearcherContext _context;

        public DetailsModel(LW12.Data.RazorPagesResearcherContext context)
        {
            _context = context;
        }

        public Projects Projects { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projects = await _context.Projects.FirstOrDefaultAsync(m => m.Id == id);

            if (projects is not null)
            {
                Projects = projects;

                return Page();
            }

            return NotFound();
        }
    }
}
