using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KGlow.Data;
using KGlow.Models;

namespace KGlow.Pages.Brands
{
    public class DetailsModel : PageModel
    {
        private readonly KGlow.Data.KGlowContext _context;

        public DetailsModel(KGlow.Data.KGlowContext context)
        {
            _context = context;
        }

      public Brand Brand { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Brand == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.ID == id);
            if (brand == null)
            {
                return NotFound();
            }
            else 
            {
                Brand = brand;
            }
            return Page();
        }
    }
}
