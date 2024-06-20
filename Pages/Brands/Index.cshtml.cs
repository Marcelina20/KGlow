using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KGlow.Data;
using KGlow.Models;
using KGlow.Models.ViewModels;
using System.Security.Policy;

namespace KGlow.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly KGlow.Data.KGlowContext _context;

        public IndexModel(KGlow.Data.KGlowContext context)
        {
            _context = context;
        }

        public IList<Brand> Brand { get;set; } = default!;


        public BrandIndexData BrandData { get; set; }
        public int BrandID { get; set; }
        public int ProductID { get; set; }
        public async Task OnGetAsync(int? id, int? productID)
        {
            BrandData = new BrandIndexData();
            BrandData.Brands = await _context.Brand
            .Include(i => i.Products)
            .OrderBy(i => i.BrandName)
            .ToListAsync();
            if (id != null)
            {
                BrandID = id.Value;
                Brand brand = BrandData.Brands
                .Where(i => i.ID == id.Value).Single();
                BrandData.Products = brand.Products;
            }
        }  
    }
}
