using Microsoft.IdentityModel.Tokens;
using Snap_N_Shop_API.DTO.CustomerDTO.Token;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Snap_N_Shop_API.Services.AuthToken
{
    public class VerifyToken
    {
        public static VerifyTokenResponse Verify(VerifyTokenRequest request, IConfiguration config)
        {
            try
            {
                var token = request.CustomerToken;
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new VerifyTokenResponse
                    {
                        Success = false,
                        Message = "Token is required"
                    };
                }
                var secretKey = config["JwtSettings:SecretKey"];
                var issuer = config["JwtSettings:Issuer"];
                var audience = config["JwtSettings:Audience"];

                if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
                {
                    return new VerifyTokenResponse
                    {
                        Success = false,
                        Message = "JwtSettings are not configured"
                    };
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

                var tokenHandler = new JwtSecurityTokenHandler();

                var validateToken = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var customerId = jwtToken.Claims.First(c => c.Type == "CustomerId").Value;
                var email = jwtToken.Claims.First(c => c.Type == "Email").Value;

                if (customerId == null || email == null)
                {
                    return new VerifyTokenResponse
                    {
                        Success = false,
                        Message = "Invalid token"
                    };
                }

                return new VerifyTokenResponse
                {
                    Success = true,
                    Message = "Token verified successfully",
                    CustomerId = customerId,
                    Email = email
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new VerifyTokenResponse
                {
                    Success = false,
                    Message = "Token verification failed"
                };
            }
        }
    }
}