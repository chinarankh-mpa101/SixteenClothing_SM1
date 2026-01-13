using System.ComponentModel.DataAnnotations;

namespace SixteenClothing.ViewModels.ProductViewModels
{
    public class ProductUpdateVM

    {
        public int Id { get; set; }
        [Required, MaxLength(256), MinLength(3)]
        public string Name { get; set; } = string.Empty;


        [Required, MaxLength(256), MinLength(3)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 5)]
        public int Rating { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public IFormFile? Image { get; set; } = null!;
    }
}
