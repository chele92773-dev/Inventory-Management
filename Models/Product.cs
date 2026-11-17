using System.ComponentModel.DataAnnotations;
namespace Inventory_Management.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; } = string.Empty;
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } = string.Empty;
        [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
        [Range(0, 1000000, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
