using Snap_N_Shop_API.Models;

namespace Snap_N_Shop_API.DTO.ProductDTO.FetchProduct
{
    public class CategoryProductRequest
    {
        public int CategoryId { get; set; }
    }
    public class CategoryProductResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = [];
    }
}