using System.ComponentModel.DataAnnotations;
namespace Online_Marketplace.Shared.DTOs
{
    public class ProductViewDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string Brand { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string BusinessName { get; set; }

        public string ImageUrl { get; set; } // Add this property for the image URL or path
    }

}
