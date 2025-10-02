using Snap_N_Shop_API.Models;

namespace Snap_N_Shop_API.DTO.OrderDTO
{
    public class FetchOrderResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<OrderItem> Orders { get; set; } = [];
    }
}