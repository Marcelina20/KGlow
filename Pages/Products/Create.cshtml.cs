using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using KGlow.Data;
using KGlow.Models;
using System.Security.Policy;

namespace KGlow.Pages.Products
{
    public class CreateModel : ProductCategoriesPageModel
    {
        private readonly KGlow.Data.KGlowContext _context;

        public CreateModel(KGlow.Data.KGlowContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["BrandID"] = new SelectList(_context.Set<Brand>(), "ID",
"BrandName");

            var product = new Product();
            product.ProductCategories = new List<ProductCategory>();
            PopulateAssignedCategoryData(_context, product);
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        { 
                var newProduct = new Product();
                if (selectedCategories != null)
                {
                    newProduct.ProductCategories = new List<ProductCategory>();
                    foreach (var cat in selectedCategories)
                    {
                        var catToAdd = new ProductCategory
                        {
                            CategoryID = int.Parse(cat)
                        };
                        newProduct.ProductCategories.Add(catToAdd);
                    }
                }
                Product.ProductCategories = newProduct.ProductCategories;
                _context.Product.Add(Product);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            
            if (!ModelState.IsValid || _context.Product == null || Product == null)
            {
                return Page();
            }

            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
