using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap_N_Shop_API.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, MaxLength(150)]
        public string ProductName { get; set; } = null!;

        [MaxLength(1000)]
        public string? ProductDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string CategoryName { get; set; } = null!;
        public Category? Category { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        [Range(0, 5)]
        [Column(TypeName = "decimal(3,1)")]
        public decimal AverageRating { get; set; } = 0m;

        public bool InStock { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}