using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KGlow.Data;
using KGlow.Models;
using System.Net;
using System.Runtime.CompilerServices;

namespace KGlow.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly KGlow.Data.KGlowContext _context;

        public IndexModel(KGlow.Data.KGlowContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;
        public ProductData ProductD { get; set; }
        public int ProductID { get; set; }
        public int CategoryID { get; set; }

        public string NameSort { get; set; }
        public string BrandSort { get; set; }
        public string CurrentSort { get; set; }
        public string CurrentFilter { get; set; }


        public async Task OnGetAsync( int? id, int? categoryID, string sortOrder, string searchString)
        {

            ProductD = new ProductData();

            //NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            NameSort = sortOrder == "name" ? "name_desc" : "name";
            BrandSort = sortOrder == "brand" ? "brand_desc" : "brand";

            CurrentFilter = searchString;

            ProductD.Products = await _context.Product
            .Include(b => b.Brand)
            .Include(b => b.ProductCategories)
            .ThenInclude(b => b.Category)
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .ToListAsync();
            //aici

            switch (sortOrder)
            {
                case "name_desc":
                    ProductD.Products = ProductD.Products.OrderByDescending(s =>
                   s.Name);
                    break;
                case "brand_desc":
                    ProductD.Products = ProductD.Products.OrderByDescending(s =>
                   s.Brand.BrandName);
                    break;
                case "brand":
                    ProductD.Products = ProductD.Products.OrderBy(s =>
                   s.Brand.BrandName);
                    break;
                default:
                    ProductD.Products = ProductD.Products.OrderBy(s => s.Name);
                    break;
            }
                    if (_context.Product != null)
            {
                Product = await _context.Product.Include(b=>b.Brand).ToListAsync();
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                ProductD.Products = ProductD.Products.Where(s => s.Brand.BrandName.Contains(searchString, StringComparison.OrdinalIgnoreCase)
               || s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (id != null)
            {
                ProductID = id.Value;
                Product product = ProductD.Products
                .Where(i => i.ID == id.Value).Single();
                ProductD.Categories = product.ProductCategories.Select(s => s.Category);
            }
        }
    }
}
