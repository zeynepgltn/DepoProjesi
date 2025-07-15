using DepoProjesi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepoProjesi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly WarehouseDbContext _context;

        public HomeController(WarehouseDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalProducts = _context.Urunler.Count();
            var lowStockProducts = _context.Urunler.Count(p => p.Stok < 10);

            ViewBag.TotalProducts = totalProducts;
            ViewBag.LowStockProducts = lowStockProducts;

            return View();
        }
    }
}
