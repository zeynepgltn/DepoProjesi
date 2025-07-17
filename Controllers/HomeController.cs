using DepoProjesi.Data;
using DepoProjesi.Models;
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
            var dusukStokLimiti = 10;

            ViewBag.TotalProducts = _context.Urunler.Count();
            ViewBag.LowStockProducts = _context.Urunler.Count(u => u.Stok < dusukStokLimiti);

            return View(); 
        }
    }
    }
