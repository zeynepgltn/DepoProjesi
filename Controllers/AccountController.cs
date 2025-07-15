using Microsoft.AspNetCore.Mvc;
using DepoProjesi.Data;
using DepoProjesi.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;

namespace DepoProjesi.Controllers
{
    public class AccountController : Controller
    {
        private readonly WarehouseDbContext _context;

        public AccountController(WarehouseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string sifre)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(sifre))
            {
                ViewBag.Hata = "Email ve şifre boş olamaz.";
                return View();
            }

            var user = _context.Personeller.FirstOrDefault(p => p.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(sifre, user.Sifre))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Rol)
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Hata = "Geçersiz giriş.";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }
    }
}
