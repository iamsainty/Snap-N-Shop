namespace Snap_N_Shop_API.DTO.CartDTO
{
    public class RemoveFromCartRequest
    {
        public int ProductId { get; set; }
    }
    public class RemoveFromCartResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}