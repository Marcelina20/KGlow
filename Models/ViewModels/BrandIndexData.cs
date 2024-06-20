using System.Security.Policy;

namespace KGlow.Models.ViewModels
{
    public class BrandIndexData
    {
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
