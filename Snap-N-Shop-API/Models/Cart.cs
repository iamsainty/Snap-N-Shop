using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap_N_Shop_API.Models
{
    public class Cart
    {
        [Key]
        public int CartItemId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}