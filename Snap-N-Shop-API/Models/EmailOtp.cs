using System;
using System.ComponentModel.DataAnnotations;

namespace Snap_N_Shop_API.Models
{
    public class EmailOtp
    {
        [Key]
        public int EmailOtpId { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(10)]
        public string OtpCode { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddMinutes(10);

        public bool IsUsed { get; set; } = false;
    }
}