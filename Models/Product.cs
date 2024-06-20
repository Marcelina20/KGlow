using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KGlow.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        public int? BrandID { get; set; }
        public Brand? Brand { get; set; }

        public ICollection<ProductCategory>? ProductCategories { get; set; }
    }
}
