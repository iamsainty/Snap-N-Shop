using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Snap_N_Shop_API.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [MaxLength(100)]
        public string? DisplayName { get; set; }

        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = null!; // consider unique index

        public bool IsProfileComplete { get; set; } = false;

        [Phone, MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string? AddressLine1 { get; set; }

        [MaxLength(200)]
        public string? AddressLine2 { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [MaxLength(100)]
        public string? State { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Navigation
        public ICollection<Cart> CartItems { get; set; } = new HashSet<Cart>();
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}