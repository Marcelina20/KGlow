using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KGlow.Data;
using KGlow.Models;
using System.Security.Policy;

namespace KGlow.Pages.Products
{
    public class EditModel : ProductCategoriesPageModel
    {
        private readonly KGlow.Data.KGlowContext _context;

        public EditModel(KGlow.Data.KGlowContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            Product = await _context.Product
             .Include(b => b.Brand)
             .Include(b => b.ProductCategories).ThenInclude(b => b.Category)
             .AsNoTracking()
             .FirstOrDefaultAsync(m => m.ID == id);

            var product =  await _context.Product.FirstOrDefaultAsync(m => m.ID == id);
            if (Product == null)
            {
                return NotFound();
            }
            //Product = product;

            PopulateAssignedCategoryData(_context, Product);
            ViewData["BrandID"] = new SelectList(_context.Set<Brand>(), "ID","BrandName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[]selectedCategories) 
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var productToUpdate = await _context.Product
            .Include(i => i.Brand)
            .Include(i => i.ProductCategories)
            .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id);

            if (productToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Product>(
            productToUpdate,
            "Product",
            i => i.Name, i => i.Brand,
            i => i.Price, i => i.ExpiryDate, i => i.BrandID))
            {
                UpdateProductCategories(_context, selectedCategories, productToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            //Apelam UpdateProductCategories pentru a aplica informatiile din checkboxuri la entitatea Products care
            //este editata
            UpdateProductCategories(_context, selectedCategories, productToUpdate);
            PopulateAssignedCategoryData(_context, productToUpdate);
            return Page();
        
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ID))
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
        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.ID == id)).GetValueOrDefault();
        } 
    }
}
