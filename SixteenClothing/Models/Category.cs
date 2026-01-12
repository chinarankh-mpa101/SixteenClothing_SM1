using SixteenClothing.Models.Common;

namespace SixteenClothing.Models
{
    public class Category:BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = [];
    }
}
