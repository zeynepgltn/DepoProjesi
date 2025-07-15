using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepoProjesi.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // Ürün listeleme sayfasını açar (List.cshtml)
        [HttpGet]
        public IActionResult List()
        {
            return View(); 
        }

        // Ürün ekleme sayfasını açar (Create.cshtml)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }

        // Güncelleme sayfası (Tüm bilgiler için)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateProductView()
        {
            return View(); 
        }

        // Sadece stok güncelleme sayfası
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult UpdateStockView()
        {
            return View();
        }

        // Silme sayfası
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DeleteProductView()
        {
            return View(); 
        }
    }
}
