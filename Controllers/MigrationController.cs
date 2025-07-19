using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DepoProjesi.Data;
using BCrypt.Net;

namespace DepoProjesi.Controllers
{
    public class MigrationController : Controller
    {
        private readonly WarehouseDbContext _context;

        public MigrationController(WarehouseDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("/Migration/HashPasswords")]
        public IActionResult HashPasswords()
        {
            var personeller = _context.Personeller.ToList();

            foreach (var personel in personeller)
            {
                // Zaten hash'li olup olmadığını kontrol etmek için basit bir uzunluk şartı
                if (personel.Sifre.Length < 20)
                {
                    personel.Sifre = BCrypt.Net.BCrypt.HashPassword(personel.Sifre);
                }
            }

            _context.SaveChanges();

            return Ok("Tüm şifreler hash'lendi.");
        }
    }
}
