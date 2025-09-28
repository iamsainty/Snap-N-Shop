using Snap_N_Shop_API.Models;

namespace Snap_N_Shop_API.DTO.CartDTO
{
    public class FetchCartResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<Cart> CartItems { get; set; } = [];
    }
}