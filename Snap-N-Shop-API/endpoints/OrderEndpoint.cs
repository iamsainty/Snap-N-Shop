using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Data;
using Snap_N_Shop_API.DTO.CustomerDTO.Token;
using Snap_N_Shop_API.DTO.OrderDTO;
using Snap_N_Shop_API.Models;
using Snap_N_Shop_API.Services.AuthToken;

namespace Snap_N_Shop_API.Endpoints
{
    public static class OrderEndpoint
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            var orderRoute = app.MapGroup("/order");

            orderRoute.MapGet("/test", () => Results.Ok("Order endpoint is working"));

            orderRoute.MapPost("/place-order", async (HttpContext context, MyDbContext db) =>
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
                        return Results.Json(new PlaceOrderResponse
                        {
                            Success = false,
                            Message = verificationResult.Message
                        });
                    }
                    var customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == verificationResult.Email);
                    if (customer == null)
                    {
                        return Results.Json(new PlaceOrderResponse
                        {
                            Success = false,
                            Message = "Customer not found"
                        });
                    }
                    var items = await db.CartItems.Where(c => c.CustomerId == customer.CustomerId).ToListAsync();
                    if (items == null || items.Count == 0)
                    {
                        return Results.Json(new PlaceOrderResponse
                        {
                            Success = false,
                            Message = "No items in cart"
                        });
                    }
                    var order = new Order
                    {
                        CustomerId = customer.CustomerId,
                        OrderDate = DateTime.UtcNow,
                        TotalAmount = items.Sum(i => i.UnitPrice * i.Quantity),
                        OrderStatus = OrderStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    };
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();
                    foreach (var item in items)
                    {
                        var orderItem = new OrderItem
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            CreatedAt = DateTime.UtcNow
                        };
                        db.OrderItems.Add(orderItem);
                        db.CartItems.Remove(item);
                    }
                    await db.SaveChangesAsync();
                    return Results.Json(new PlaceOrderResponse
                    {
                        Success = true,
                        Message = "Order placed successfully"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new PlaceOrderResponse
                    {
                        Success = false,
                        Message = "Unexpected error"
                    });
                }
            });

            orderRoute.MapGet("/fetch-orders", async (HttpContext context, MyDbContext db) =>
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
                        return Results.Json(new FetchOrderResponse
                        {
                            Success = false,
                            Message = verificationResult.Message
                        });
                    }

                    var customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == verificationResult.Email);
                    if (customer == null)
                    {
                        return Results.Json(new FetchOrderResponse
                        {
                            Success = false,
                            Message = "Customer not found"
                        });
                    }
                    var orders = await db.Orders.Where(o => o.CustomerId == customer.CustomerId).ToListAsync();
                    if (orders == null || orders.Count == 0)
                    {
                        return Results.Json(new FetchOrderResponse
                        {
                            Success = false,
                            Message = "No orders found",
                            Orders = []
                        });
                    }
                    return Results.Json(new FetchOrderResponse
                    {
                        Success = true,
                        Message = "Orders fetched successfully",
                        Orders = orders
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new FetchOrderResponse
                    {
                        Success = false,
                        Message = "Unexpected error"
                    });
                }
            });
        }
    }
}