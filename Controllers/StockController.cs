using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepoProjesi.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        // GET: /Stock/Tracking
        public IActionResult Tracking()
        {
            // StockTracking.cshtml dosyasını aç
            return View("StockTracking");
        }
    }
}
