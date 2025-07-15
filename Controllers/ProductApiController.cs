using ClosedXML.Excel;
using DepoProjesi.Data;
using DepoProjesi.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductApiController : ControllerBase
{
    private readonly WarehouseDbContext _context;

    public ProductApiController(WarehouseDbContext context)
    {
        _context = context;
    }

    // CREATE
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        product.EklenmeTarihi = DateTime.Now;
        _context.Urunler.Add(product);
        await _context.SaveChangesAsync();

        return Ok(product);
    }


    // READ (Tüm Ürünleri Listele)
    [HttpGet("list")]
    public IActionResult GetAll(string? arama, string? kategori, bool? stoktaVarOlanlar)
    {
        var query = _context.Urunler.AsQueryable();

        if (!string.IsNullOrEmpty(arama))
            query = query.Where(p => p.UrunAdi.Contains(arama));

        if (!string.IsNullOrEmpty(kategori))
            query = query.Where(p => p.Kategori == kategori);

        if (stoktaVarOlanlar.HasValue && stoktaVarOlanlar.Value)
            query = query.Where(p => p.Stok > 0);

        var result = query.ToList();
        return Ok(result);
    }
    [HttpGet("kategoriler")]
    public IActionResult Kategoriler()
    {
        var kategoriler = _context.Urunler
            .Select(u => u.Kategori)
            .Distinct()
            .ToList();
        return Ok(kategoriler);
    }
    [HttpGet("export")]
    public IActionResult ExportToExcel()
    {
        var urunler = _context.Urunler.ToList();

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Ürünler");

        // Başlıklar
        ws.Cell(1, 1).Value = "ID";
        ws.Cell(1, 2).Value = "Ürün Adı";
        ws.Cell(1, 3).Value = "Kategori";
        ws.Cell(1, 4).Value = "Fiyat";
        ws.Cell(1, 5).Value = "Stok";

        for (int i = 0; i < urunler.Count; i++)
        {
            var u = urunler[i];
            ws.Cell(i + 2, 1).Value = u.UrunId;
            ws.Cell(i + 2, 2).Value = u.UrunAdi;
            ws.Cell(i + 2, 3).Value = u.Kategori;
            ws.Cell(i + 2, 4).Value = u.Fiyat;
            ws.Cell(i + 2, 5).Value = u.Stok;
        }

        ws.ColumnsUsed().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Urunler.xlsx");
    }



    // READ (ID ile)
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var product = _context.Urunler.Find(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }


    // UPDATE
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product updated)
    {
        var product = await _context.Urunler.FindAsync(id);
        if (product == null)
            return NotFound();

        product.UrunAdi = updated.UrunAdi;
        product.Kategori = updated.Kategori;
        product.Fiyat = updated.Fiyat;
        product.Stok = updated.Stok;
        product.EklenmeTarihi = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(product);
    }

    public class UpdateStockDto
    {
        public int Stok { get; set; }
    }

    [HttpPatch("updatestock/{id}")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockDto body)
    {
        var product = await _context.Urunler.FindAsync(id);
        if (product == null)
            return NotFound("Ürün bulunamadı.");

        product.Stok = body.Stok;
        product.EklenmeTarihi = DateTime.Now;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Stok güncellendi." });
    }


    // DELETE
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Urunler.FindAsync(id);
        if (product == null)
            return NotFound();

        _context.Urunler.Remove(product);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Ürün silindi" });
    }
}
