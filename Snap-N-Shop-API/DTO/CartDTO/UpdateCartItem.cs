namespace Snap_N_Shop_API.DTO.CartDTO
{
    public class UpdateCartItemRequest
    {
        public int ProductId { get; set; }
        public bool IncQty { get; set; } = false;
    }
    public class UpdateCartItemResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}