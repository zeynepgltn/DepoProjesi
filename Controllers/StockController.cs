using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepoProjesi.Controllers
{
    [Authorize]
    [Authorize]
    public class StockController : Controller
    {
        public IActionResult Tracking()
        {
            return View("StockTracking");
        }

        public IActionResult Chart()
        {
            return View("StockChart");
        }

        public IActionResult Tuketim()
        {
            return View("Consumed");
        }

    }

}
