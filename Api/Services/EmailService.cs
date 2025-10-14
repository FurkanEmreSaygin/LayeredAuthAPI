using Business.Services;
using Business.Settings; // Yeni ayarlar modelini kullanmak için
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        // Constructor: Ayarları IOptionsPattern ile alıyoruz.
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendVerificationEmailAsync(User user, string verificationLink)
        {
            try
            {
                var emailBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px; text-align: center; color: #333;'>
                    <h2 style='color: #4CAF50;'>FoundationAuth API</h2>
                    <p>Merhaba {user.Username},</p>
                    <p>Hesabınızı doğrulamak için lütfen aşağıdaki butona tıklayın.</p>
                    <table style='margin: 30px auto; border-collapse: collapse;'>
                        <tr>
                            <td style='padding: 0;'>
                            <a href='{verificationLink}' style='
                                display: inline-block;
                                padding: 12px 24px;
                                background-color: #007BFF;
                                color: #ffffff;
                                text-decoration: none;
                                border-radius: 5px;
                                font-weight: bold;
                                '>Hesabımı Doğrula</a>
                            </td>
                        </tr>
                    </table>
                    <p style='color: #888; font-size: 12px;'>Bu bağlantı 24 saat geçerlidir. Eğer siz talep etmediyseniz bu e-postayı görmezden gelebilirsiniz.</p>
                    <p style='color: #888; font-size: 12px;'>API Geliştirme Takımı</p>
                    </div>
                ";
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, "FoundationAuth Destek"),
                    Subject = "Hesabınızı Doğrulayın - FoundationAuth API",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(user.Email);

                using (var smtpClient = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                    smtpClient.EnableSsl = _emailSettings.EnableSsl;
                    await smtpClient.SendMailAsync(mailMessage);
                }

                Console.WriteLine($"E-POSTA BAŞARIYLA GÖNDERİLDİ: {user.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"E-POSTA GÖNDERİMİ BAŞARISIZ: {ex.Message}");
                throw new Exception("E-posta gönderimi sırasında bir hata oluştu. Lütfen ayarlarınızı kontrol edin.");
            }

        }
    }
}