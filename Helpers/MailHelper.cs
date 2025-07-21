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

        public async Task SendLowStockSummaryAsync(List<(string urunAdi, string kategori, int stok)> urunler)
        {
            try
            {
                if (urunler == null || urunler.Count == 0)
                    return;

                var smtpClient = new SmtpClient(_config["EmailSettings:SmtpServer"])
                {
                    Port = int.Parse(_config["EmailSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _config["EmailSettings:SenderEmail"],
                        _config["EmailSettings:SenderPassword"]
                    ),
                    EnableSsl = true,
                };

                string body = " Aşağıdaki ürünlerin stoğu kritik seviyeye düşmüştür:\n\n";

                foreach (var u in urunler)
                {
                    body += $"- {u.urunAdi} ({u.kategori}) → Stok: {u.stok}\n";
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:SenderEmail"]),
                    Subject = " Kritik Stok Özeti",
                    Body = body,
                    IsBodyHtml = false,
                };

                mailMessage.To.Add("zeynepgultenn@gmail.com");

                await smtpClient.SendMailAsync(mailMessage);
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
