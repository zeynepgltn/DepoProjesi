using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace DepoProjesi.Helpers
{
    public class MailHelper
    {
        private readonly IConfiguration _config;

        public MailHelper(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendLowStockAlertAsync(string toEmail, string urunAdi, int mevcutStok)
        {
            try
            {
                Console.WriteLine("📤 Mail gönderimi başlıyor...");

                var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
                {
                    Port = int.Parse(_config["EmailSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _config["EmailSettings:SenderEmail"],
                        _config["EmailSettings:SenderPassword"]
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:SenderEmail"]),
                    Subject = "🔴 Kritik Stok Uyarısı",
                    Body = $"'{urunAdi}' ürününün stoğu kritik seviyeye ({mevcutStok}) düştü.",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);

                Console.WriteLine("✅ Mail başarıyla gönderildi.");
            }
            catch (Exception ex)
            {
                var log = $"[MailHATA] {DateTime.Now}: {ex.Message}\n{ex.InnerException?.Message}";
                File.AppendAllText("mail_hata_log.txt", log + "\n\n");
                throw;
            }

        }
    }
}
//         {