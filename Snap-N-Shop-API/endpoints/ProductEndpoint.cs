using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Data;
using Snap_N_Shop_API.DTO.ProductDTO.FetchProduct;

namespace Snap_N_Shop_API.Endpoints
{
    public static class ProductEndpoint
    {
        public static void MapProductEndpoints(this WebApplication app)
        {
            var productRoute = app.MapGroup("/product");

            productRoute.MapGet("/test", () => Results.Ok("Product endpoint is working"));

            productRoute.MapGet("/all-product", async (MyDbContext db) =>
            {
                try
                {
                    // first 15 products
                    // var products = await db.Products.Take(15).ToListAsync();
                    var products = await db.Products.ToListAsync();
                    return Results.Json(new AllProductResponse
                    {
                        Success = true,
                        Message = "Products fetched successfully",
                        Products = products ?? []
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new AllProductResponse
                    {
                        Success = false,
                        Message = "Unexpected error",
                        Products = []
                    });
                }
            });

            productRoute.MapPost("/product-details", async (ProductDetailsRequest request, MyDbContext db) =>
            {
                try
                {
                    var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == request.ProductId);
                    if (product == null)
                    {
                        return Results.Json(new ProductDetailsResponse
                        {
                            Success = false,
                            Message = "Product not found",
                            Product = null
                        });
                    }
                    return Results.Json(new ProductDetailsResponse
                    {
                        Success = true,
                        Message = "Product details fetched successfully",
                        Product = product
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new ProductDetailsResponse
                    {
                        Success = false,
                        Message = "Unexpected error",
                        Product = null
                    });
                }
            });

            productRoute.MapPost("/category-product", async (CategoryProductRequest request, MyDbContext db) =>
            {
                try
                {
                    string categoryName = request.CategoryName.ToLower();
                    var products = await db.Products.Where(p => p.CategoryName == categoryName).ToListAsync();
                    if (products == null)
                    {
                        return Results.Json(new CategoryProductResponse
                        {
                            Success = false,
                            Message = "Products not found",
                            Products = []
                        });
                    }
                    return Results.Json(new CategoryProductResponse
                    {
                        Success = true,
                        Message = "Products fetched successfully",
                        Products = products
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new CategoryProductResponse
                    {
                        Success = false,
                        Message = "Unexpected error",
                        Products = []
                    });
                }
            });

            productRoute.MapPost("/search-product", async (SearchProductRequest request, MyDbContext db) =>
            {
                try
                {
                    var products = await db.Products.Where(p => p.ProductName.Contains(request.SearchQuery)).ToListAsync();
                    if (products == null)
                    {
                        return Results.Json(new SearchProductResponse
                        {
                            Success = false,
                            Message = "Products not found",
                            Products = []
                        });
                    }
                    return Results.Json(new SearchProductResponse
                    {
                        Success = true,
                        Message = "Products fetched successfully",
                        Products = products
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new SearchProductResponse
                    {
                        Success = false,
                        Message = "Unexpected error",
                        Products = []
                    });
                }
            });
        }
    }
}