using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Data;
using Snap_N_Shop_API.DTO.CartDTO;
using Snap_N_Shop_API.DTO.CustomerDTO.Token;
using Snap_N_Shop_API.Services.AuthToken;
using Snap_N_Shop_API.Models;

namespace Snap_N_Shop_API.Endpoints
{
    public static class CartEndpoint
    {
        public static void MapCartEndpoints(this WebApplication app)
        {
            var cartRoute = app.MapGroup("/cart");

            cartRoute.MapGet("/test", () => Results.Ok("Cart endpoint is working"));

            cartRoute.MapGet("/fetch-cart", async (HttpContext context, MyDbContext db) =>
            {
                try
                {
                    var authHeader = context.Request.Headers.Authorization.ToString();
                    var customerToken = authHeader.StartsWith("Bearer ") ? authHeader[7..] : authHeader;

                    var verificationResult = VerifyToken.Verify(new VerifyTokenRequest
                    {
                        CustomerToken = customerToken
                    }, app.Configuration);
                    if (!verificationResult.Success)
                    {
                        return Results.Json(new FetchCartResponse
                        {
                            Success = false,
                            Message = verificationResult.Message
                        });
                    }
                    var customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == verificationResult.Email);
                    if (customer == null)
                    {
                        return Results.Json(new FetchCartResponse
                        {
                            Success = false,
                            Message = "Customer not found"
                        });
                    }
                    var cartItems = await db.CartItems.Where(c => c.CustomerId == customer.CustomerId).ToListAsync();
                    if (cartItems == null)
                    {
                        return Results.Json(new FetchCartResponse
                        {
                            Success = false,
                            Message = "Cart items not found"
                        });
                    }
                    return Results.Json(new FetchCartResponse
                    {
                        Success = true,
                        Message = "Cart items fetched successfully",
                        CartItems = cartItems
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new FetchCartResponse
                    {
                        Success = false,
                        Message = "Unexpected error",
                        CartItems = []
                    });
                }
            });

            cartRoute.MapPost("/add-to-cart", async (AddToCartRequest request, HttpContext context, MyDbContext db) =>
            {
                try
                {
                    var authHeader = context.Request.Headers.Authorization.ToString();
                    var customerToken = authHeader.StartsWith("Bearer ") ? authHeader[7..] : authHeader;

                    var verificationResult = VerifyToken.Verify(new VerifyTokenRequest
                    {
                        CustomerToken = customerToken
                    }, app.Configuration);
                    if (!verificationResult.Success)
                    {
                        return Results.Json(new AddToCartResponse
                        {
                            Success = false,
                            Message = verificationResult.Message
                        });
                    }
                    var customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == verificationResult.Email);
                    if (customer == null)
                    {
                        return Results.Json(new AddToCartResponse
                        {
                            Success = false,
                            Message = "Customer not found"
                        });
                    }
                    var cartItem = await db.CartItems.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId && c.ProductId == request.ProductId);
                    if (cartItem != null)
                    {
                        return Results.Json(new AddToCartResponse
                        {
                            Success = false,
                            Message = "Cart item already exists"
                        });
                    }
                    var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == request.ProductId);
                    if (product == null)
                    {
                        return Results.Json(new AddToCartResponse
                        {
                            Success = false,
                            Message = "Product not found"
                        });
                    }
                    cartItem = new Cart
                    {
                        CustomerId = customer.CustomerId,
                        Customer = customer,
                        ProductId = request.ProductId,
                        Product = product,
                        UnitPrice = product.Price,
                        Quantity = 1,
                        CreatedAt = DateTime.UtcNow
                    };
                    db.CartItems.Add(cartItem);
                    await db.SaveChangesAsync();
                    return Results.Json(new AddToCartResponse
                    {
                        Success = true,
                        Message = "Cart item added successfully"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new AddToCartResponse
                    {
                        Success = false,
                        Message = "Unexpected error"
                    });
                }
            });

            cartRoute.MapPost("/remove-from-cart", async (RemoveFromCartRequest request, HttpContext context, MyDbContext db) =>
            {
                try
                {
                    var authHeader = context.Request.Headers.Authorization.ToString();
                    var customerToken = authHeader.StartsWith("Bearer ") ? authHeader[7..] : authHeader;

                    var verificationResult = VerifyToken.Verify(new VerifyTokenRequest
                    {
                        CustomerToken = customerToken
                    }, app.Configuration);
                    if (!verificationResult.Success)
                    {
                        return Results.Json(new RemoveFromCartResponse
                        {
                            Success = false,
                            Message = verificationResult.Message
                        });
                    }
                    var customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == verificationResult.Email);
                    if (customer == null)
                    {
                        return Results.Json(new RemoveFromCartResponse
                        {
                            Success = false,
                            Message = "Customer not found"
                        });
                    }
                    var cartItem = await db.CartItems.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId && c.ProductId == request.ProductId);
                    if (cartItem == null)
                    {
                        return Results.Json(new RemoveFromCartResponse
                        {
                            Success = false,
                            Message = "Cart item not found"
                        });
                    }
                    db.CartItems.Remove(cartItem);
                    await db.SaveChangesAsync();
                    return Results.Json(new RemoveFromCartResponse
                    {
                        Success = true,
                        Message = "Cart item removed successfully"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new RemoveFromCartResponse
                    {
                        Success = false,
                        Message = "Unexpected error"
                    });
                }
            });

            cartRoute.MapPost("/update-cart-item", async (UpdateCartItemRequest request, HttpContext context, MyDbContext db) =>
            {
                try
                {
                    var authHeader = context.Request.Headers.Authorization.ToString();
                    var customerToken = authHeader.StartsWith("Bearer ") ? authHeader[7..] : authHeader;

                    var verificationResult = VerifyToken.Verify(new VerifyTokenRequest
                    {
                        CustomerToken = customerToken
                    }, app.Configuration);
                    if (!verificationResult.Success)
                    {
                        return Results.Json(new UpdateCartItemResponse
                        {
                            Success = false,
                            Message = verificationResult.Message
                        });
                    }
                    var customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == verificationResult.Email);
                    if (customer == null)
                    {
                        return Results.Json(new UpdateCartItemResponse
                        {
                            Success = false,
                            Message = "Customer not found"
                        });
                    }
                    var cartItem = await db.CartItems.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId && c.ProductId == request.ProductId);
                    if (cartItem == null)
                    {
                        return Results.Json(new UpdateCartItemResponse
                        {
                            Success = false,
                            Message = "Cart item not found"
                        });
                    }
                    if (request.IncQty)
                    {
                        cartItem.Quantity++;
                    }
                    else
                    {
                        cartItem.Quantity--;
                    }
                    await db.SaveChangesAsync();
                    return Results.Json(new UpdateCartItemResponse
                    {
                        Success = true,
                        Message = "Cart item updated successfully"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new UpdateCartItemResponse
                    {
                        Success = false,
                        Message = "Unexpected error"
                    });
                }
            });
        }
    }
}