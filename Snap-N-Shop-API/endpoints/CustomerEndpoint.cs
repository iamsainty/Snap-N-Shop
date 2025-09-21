using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Models;
using Snap_N_Shop_API.Services;
using Snap_N_Shop_API.Data;

namespace Snap_N_Shop_API.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void MapCustomerEndpoints(this WebApplication app)
        {
            var customerRoute = app.MapGroup("/customer");

            customerRoute.MapPost("/send-otp", async (string email, EmailService emailService, MyDbContext db) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(email))
                        return Results.Json(new { success = false, message = "Email is required" });

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
                        return Results.Json(new { success = true, message = $"OTP sent to {email}" });
                    else
                        return Results.Json(new { success = false, message = "Failed to send OTP" });

                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new { success = false, message = "Database error" });
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new { success = false, message = "Invalid operation" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new { success = false, message = "Unexpected error" });
                }
            });
        }
    }
}