using Microsoft.AspNetCore.Http;
using Online_Marketplace.DAL.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Marketplace.DAL.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string Brand { get; set; }

        [ForeignKey(nameof(Seller))]
        public int SellerId { get; set; }

        public Seller Seller { get; set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public string ImagePath { get; set; } // Store the file path instead of the IFormFile

        public virtual ICollection<ProductReviews> ProductReview { get; set; }
    }

}