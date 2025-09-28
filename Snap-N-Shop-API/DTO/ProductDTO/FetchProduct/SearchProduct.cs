using Snap_N_Shop_API.Models;

namespace Snap_N_Shop_API.DTO.ProductDTO.FetchProduct
{
    public class SearchProductRequest
    {
        public string SearchQuery { get; set; } = string.Empty;
    }
    public class SearchProductResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = [];
    }
}