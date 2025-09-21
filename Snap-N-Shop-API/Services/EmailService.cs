using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using MailKit.Security;

namespace Snap_N_Shop_API.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendOtpEmail(string toEmail, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Snap N Shop", _smtpSettings.UserName));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Your OTP Code";
            message.Body = new TextPart("html")
            {
                Text = $@"
                    <div style=""font-family: Arial, sans-serif; font-size: 16px; color: #333;"">
                        <p>Hi there,</p>
                        <p>We received a request to sign in to your <strong>Snap N Shop</strong> account.</p>
                        
                        <p>Your One-Time Password (OTP) is:</p>
                        
                        <span style=""display: inline-block; font-size: 36px; font-weight: bold; letter-spacing: 5px; margin: 10px;"">
                            {otp}
                        </span>
                        
                        <p>This OTP is valid for <strong>10 minutes</strong>. Please do not share it with anyone.</p>
                        
                        <p style=""font-size: 14px; color: #777; margin-top: 20px;"">
                            If you did not request this OTP, you can safely ignore this email.
                        </p>
                        
                        <p style=""font-size: 14px; color: #777; margin-top: 5px;"">— The Snap N Shop Team</p>
                    </div>"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}