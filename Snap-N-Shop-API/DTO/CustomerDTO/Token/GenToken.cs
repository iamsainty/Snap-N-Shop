namespace Snap_N_Shop_API.DTO.CustomerDTO.Token
{
    public class GenTokenRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class GenTokenResponse
    {
        public bool Success { get; set; } = false;
        public string CustomerToken { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}