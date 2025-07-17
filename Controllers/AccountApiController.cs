using Microsoft.AspNetCore.Mvc;
using DepoProjesi.Data;
using DepoProjesi.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AccountApiController : ControllerBase
{
    private readonly WarehouseDbContext _context;

    public AccountApiController(WarehouseDbContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        if (string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Sifre))
        {
            return BadRequest(new { success = false, message = "Boş alan bırakmayın." });
        }

        var user = _context.Personeller.FirstOrDefault(p => p.Email == login.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(login.Sifre, user.Sifre))
        {
            return Unauthorized(new { success = false, message = "Geçersiz kullanıcı veya şifre." });
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Rol)
        };

        var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
        await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

        return Ok(new { success = true, message = "Giriş başarılı." });
    }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Sifre { get; set; }
}
