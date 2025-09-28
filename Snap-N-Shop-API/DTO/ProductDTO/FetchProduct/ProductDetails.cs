using Snap_N_Shop_API.Models;
namespace Snap_N_Shop_API.DTO.ProductDTO.FetchProduct
{
    public class ProductDetailsRequest
    {
        public int ProductId { get; set; }
    }
    public class ProductDetailsResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public Product? Product { get; set; } = null;
    }
}