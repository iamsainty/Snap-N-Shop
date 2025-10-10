using Resend;

namespace Snap_N_Shop_API.Services
{
    public class EmailService
    {
        private readonly ResendClient _resendClient;

        public EmailService(IConfiguration config)
        {
            var resendApiKey = config["ResendAPIKEY"] ?? string.Empty;
            _resendClient = (ResendClient)ResendClient.Create(resendApiKey);
        }

        public async Task<bool> SendOtpEmail(string toEmail, string otp)
        {
            try
            {
                var response = await _resendClient.EmailSendAsync(new EmailMessage()
                {
                    From = "Snap N Shop <onboarding@resend.dev>",
                    To = new[] { toEmail },
                    Subject = "Your OTP Code",
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; font-size: 16px; color: #333;'>
                            <p>Hi there,</p>
                            <p>We received a request to sign in to your <strong>Snap N Shop</strong> account.</p>
                            
                            <p>Your One-Time Password (OTP) is:</p>
                            
                            <span style='display: inline-block; font-size: 36px; font-weight: bold; letter-spacing: 5px; margin: 10px;'>
                                {otp}
                            </span>
                            
                            <p>This OTP is valid for <strong>10 minutes</strong>. Please do not share it with anyone.</p>
                            
                            <p style='font-size: 14px; color: #777; margin-top: 20px;'>
                                If you did not request this OTP, you can safely ignore this email.
                            </p>
                            
                            <p style='font-size: 14px; color: #777; margin-top: 5px;'>— The Snap N Shop Team</p>
                        </div>"
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending OTP email: " + ex.Message);
                return false;
            }
        }
    }
}