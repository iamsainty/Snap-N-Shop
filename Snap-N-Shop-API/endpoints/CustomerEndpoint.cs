using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Models;
using Snap_N_Shop_API.Services;
using Snap_N_Shop_API.Data;
using Snap_N_Shop_API.DTO.CustomerDTO;

namespace Snap_N_Shop_API.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void MapCustomerEndpoints(this WebApplication app)
        {
            var customerRoute = app.MapGroup("/customer");

            customerRoute.MapGet("/test", () => Results.Ok("Customer endpoint is working"));

            customerRoute.MapPost("/send-otp", async (SendOtpRequest request, EmailService emailService, MyDbContext db) =>
            {
                try
                {
                    var email = request.Email;
                    if (string.IsNullOrWhiteSpace(email))
                        return Results.Json(new SendOtpResponse
                        {
                            Success = false,
                            Message = "Email is required"
                        });

                    var otp = new Random().Next(100000, 999999).ToString();

                    var emailOtp = new EmailOtp
                    {
                        Email = email,
                        OtpCode = otp,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                        IsUsed = false
                    };

                    db.EmailOtps.Add(emailOtp);
                    await db.SaveChangesAsync();

                    bool result = await emailService.SendOtpEmail(email, otp);

                    if (result)
                    {
                        return Results.Json(new SendOtpResponse
                        {
                            Success = true,
                            Message = $"OTP sent to {email}"
                        });
                    }
                    else
                        return Results.Json(new SendOtpResponse
                        {
                            Success = false,
                            Message = "Failed to send OTP"
                        });

                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);

                    return Results.Json(new SendOtpResponse
                    {
                        Success = false,
                        Message = "Database error"
                    });
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new SendOtpResponse
                    {
                        Success = false,
                        Message = "Invalid operation"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new SendOtpResponse
                    {
                        Success = false,
                        Message = "Unexpected error"
                    });
                }
            });
        }
    }
}