using DepoProjesi.Data;
using Microsoft.EntityFrameworkCore;

namespace DepoProjesi.Helpers
{
    public class StockChecker
    {
        private readonly WarehouseDbContext _context;
        private readonly MailHelper _mailHelper;

        public StockChecker(WarehouseDbContext context, MailHelper mailHelper)
        {
            _context = context;
            _mailHelper = mailHelper;
        }

        public async Task CheckAndSendAsync()
        {
            var threshold = 10;

            var dusukStoklar = await _context.Urunler
                .Where(u => u.Stok <= threshold)
                .Select(u => new { u.UrunAdi, u.Kategori, u.Stok })
                .ToListAsync();

            var liste = dusukStoklar
                .Select(x => (x.UrunAdi, x.Kategori, x.Stok))
                .ToList();

            await _mailHelper.SendLowStockSummaryAsync(liste);
        }
    }
}
