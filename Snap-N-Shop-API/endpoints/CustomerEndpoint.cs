using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Models;
using Snap_N_Shop_API.Services;
using Snap_N_Shop_API.Data;
using Snap_N_Shop_API.DTO.CustomerDTO.Token;
using Snap_N_Shop_API.DTO.CustomerDTO.OtpAuth;
using Snap_N_Shop_API.Services.AuthToken;
using Snap_N_Shop_API.DTO.CustomerDTO.Profile;

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

                    var oldOtps = db.EmailOtps.Where(e => e.Email == email && !e.IsUsed);
                    db.EmailOtps.RemoveRange(oldOtps);
                    await db.SaveChangesAsync();

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

            customerRoute.MapPost("/verify-otp", async (VerifyOtpRequest request, MyDbContext db) =>
            {
                try
                {
                    var email = request.Email;
                    var otp = request.Otp;

                    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
                    {
                        return Results.Json(new VerifyOtpResponse
                        {
                            Success = false,
                            Message = "Email and OTP are required"
                        });
                    }

                    var emailOtp = await db.EmailOtps.Where(e => e.Email == email).OrderByDescending(e => e.CreatedAt).FirstOrDefaultAsync();

                    if (emailOtp == null)
                    {
                        return Results.Json(new VerifyOtpResponse
                        {
                            Success = false,
                            Message = "Email not found"
                        });
                    }

                    if (emailOtp.OtpCode != otp)
                    {
                        return Results.Json(new VerifyOtpResponse
                        {
                            Success = false,
                            Message = "Invalid OTP"
                        });
                    }

                    if (emailOtp.ExpiresAt < DateTime.UtcNow)
                    {
                        return Results.Json(new VerifyOtpResponse
                        {
                            Success = false,
                            Message = "OTP expired"
                        });
                    }

                    if (emailOtp.IsUsed)
                    {
                        return Results.Json(new VerifyOtpResponse
                        {
                            Success = false,
                            Message = "OTP already used"
                        });
                    }

                    emailOtp.IsUsed = true;
                    await db.SaveChangesAsync();

                    var customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == email);

                    if (customer == null)
                    {
                        customer = new Customer
                        {
                            Email = email,
                            IsProfileComplete = false,
                            CreatedAt = DateTime.UtcNow
                        };
                        db.Customers.Add(customer);
                        await db.SaveChangesAsync();
                    }

                    var customerToken = GenerateToken.Generate(new GenTokenRequest
                    {
                        CustomerId = customer.CustomerId.ToString(),
                        Email = email
                    },
                        app.Configuration
                    );

                    return Results.Json(new VerifyOtpResponse
                    {
                        Success = true,
                        CustomerToken = customerToken.CustomerToken,
                        Message = "OTP verified"
                    });

                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new VerifyOtpResponse
                    {
                        Success = false,
                        Message = "Database error"
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new VerifyOtpResponse
                    {
                        Success = false,
                        Message = "Unexpected error"
                    });
                }
            });

            customerRoute.MapGet("/fetch-customer", async (HttpContext context, MyDbContext db) =>
            {
                try
                {
                    var authHeader = context.Request.Headers.Authorization.ToString();
                    var customerToken = authHeader.StartsWith("Bearer ") ? authHeader[7..] : authHeader;

                    var verificationResult = VerifyToken.Verify(new VerifyTokenRequest
                    {
                        CustomerToken = customerToken
                    },
                        app.Configuration
                    );
                    if (!verificationResult.Success)
                    {
                        return Results.Json(new FetchCustomerResponse
                        {
                            Success = false,
                            Message = verificationResult.Message
                        });
                    }
                    var customer = await db.Customers.FirstOrDefaultAsync(c => c.Email == verificationResult.Email);
                    if (customer == null)
                    {
                        return Results.Json(new FetchCustomerResponse
                        {
                            Success = false,
                            Message = "Customer not found"
                        });
                    }
                    return Results.Json(new FetchCustomerResponse
                    {
                        Success = true,
                        Message = "Customer fetched successfully",
                        Email = customer.Email,
                        DisplayName = customer.DisplayName ?? string.Empty,
                        PhoneNumber = customer.PhoneNumber ?? string.Empty,
                        AddressLine1 = customer.AddressLine1 ?? string.Empty,
                        AddressLine2 = customer.AddressLine2 ?? string.Empty,
                        City = customer.City ?? string.Empty,
                        State = customer.State ?? string.Empty,
                        PostalCode = customer.PostalCode ?? string.Empty,
                        Country = customer.Country ?? string.Empty,
                        IsProfileComplete = customer.IsProfileComplete,
                        CreatedAt = customer.CreatedAt.DateTime
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Results.Json(new FetchCustomerResponse
                    {
                        Success = false,
                        Message = "Unexpected error"
                    });
                }
            });
        }
    }
}