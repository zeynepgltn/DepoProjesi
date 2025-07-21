using DepoProjesi.Data;
using DepoProjesi.Helpers; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DepoProjesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockApiController : ControllerBase
    {
        private readonly WarehouseDbContext _context;
        private readonly MailHelper _mailHelper;

        public StockApiController(WarehouseDbContext context, MailHelper mailHelper)
        {
            _context = context;
            _mailHelper = mailHelper;
        }

        // GET: /api/StockApi/low-stock
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var threshold = 10;

            var result = await _context.Urunler
                .Where(u => u.Stok <= threshold)
                .Select(u => new
                {
                    urunAdi = u.UrunAdi,
                    kategori = u.Kategori,
                    stok = u.Stok
                })
                .ToListAsync();

            // Eğer kritik ürün varsa, yöneticiyi bilgilendir
            if (result.Any())
            {
                foreach (var urun in result)
                {
                    await _mailHelper.SendLowStockAlertAsync(
                        "zeynepgultenn@gmail.com",  // uyarı alacak kişi
                        urun.urunAdi,
                        urun.stok
                    );
                }
            }

            return Ok(result);
        }

        // GET: /api/StockApi/stock-changes
        [HttpGet("stock-changes")]
        public async Task<IActionResult> GetStockChanges()
        {
            var result = await _context.StockGecmisi
                .OrderByDescending(s => s.GuncellenmeTarihi)
                .Take(20)
                .Select(s => new
                {
                    urunAdi = s.UrunAdi,
                    degisim = s.YeniStok - s.OncekiStok,
                    tarih = s.GuncellenmeTarihi.ToString("dd.MM.yyyy HH:mm")
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: /api/StockApi/stock-changes-chart
        [HttpGet("stock-changes-chart")]
        public async Task<IActionResult> GetStockChangesChart()
        {
            var startDate = DateTime.Today.AddDays(-6); // Son 7 gün
            var endDate = DateTime.Today.AddDays(1);

            var rawData = await _context.StockGecmisi
                .Where(s => s.GuncellenmeTarihi >= startDate && s.GuncellenmeTarihi < endDate)
                .ToListAsync();

            var grouped = rawData
                .GroupBy(s => s.GuncellenmeTarihi.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    tarih = g.Key.ToString("dd.MM"),
                    toplamDegisim = g.Sum(x => x.YeniStok - x.OncekiStok)
                })
                .ToList();

            return Ok(grouped);
        }
    }
}
