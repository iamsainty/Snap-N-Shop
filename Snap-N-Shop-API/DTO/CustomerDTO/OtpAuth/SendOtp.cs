using System.ComponentModel.DataAnnotations;

namespace Snap_N_Shop_API.DTO.CustomerDTO.OtpAuth
{
    public class SendOtpRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }

    public class SendOtpResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
