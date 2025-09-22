namespace Snap_N_Shop_API.DTO.CustomerDTO.Token
{
    public class VerifyTokenRequest
    {
        public string CustomerToken { get; set; } = string.Empty;
    }

    public class VerifyTokenResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}