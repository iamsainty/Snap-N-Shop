using Snap_N_Shop_API.DTO.CustomerDTO.Token;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Snap_N_Shop_API.Services
{
    public class GenerateToken
    {
        public static GenTokenResponse Generate(GenTokenRequest request, IConfiguration config)
        {
            try
            {
                var secretKey = config["JwtSettings:SecretKey"];
                var issuer = config["JwtSettings:Issuer"];
                var audience = config["JwtSettings:Audience"];

                if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
                {
                    return new GenTokenResponse
                    {
                        Success = false,
                        CustomerToken = string.Empty,
                        Message = "JwtSettings are not configured"
                    };
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("CustomerId", request.CustomerId),
                    new Claim("Email", request.Email)
                };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return new GenTokenResponse
                {
                    Success = true,
                    CustomerToken = tokenString,
                    Message = "Token generated successfully"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new GenTokenResponse
                {
                    Success = false,
                    CustomerToken = string.Empty,
                    Message = "Token generation failed"
                };
            }
        }
    }
}