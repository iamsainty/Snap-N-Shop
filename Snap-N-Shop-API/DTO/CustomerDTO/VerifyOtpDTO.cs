using System.ComponentModel.DataAnnotations;

namespace Snap_N_Shop_API.DTO.CustomerDTO
{
    public class VerifyOtpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(6)]
        [MinLength(6)]
        public string Otp { get; set; } = string.Empty;
    }

    public class VerifyOtpResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}