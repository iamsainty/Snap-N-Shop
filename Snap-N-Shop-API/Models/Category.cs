using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snap_N_Shop_API.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = null!;

        [MaxLength(500)]
        public string? CategoryDescription { get; set; }

        [MaxLength(500)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}