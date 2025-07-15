using System.ComponentModel.DataAnnotations;

namespace DepoProjesi.Models
{
    public class Product
    {
        [Key]
        public int UrunId { get; set; }

        public required string UrunAdi { get; set; }
        public decimal Fiyat { get; set; }
        public int Stok { get; set; }
        public required string Kategori { get; set; }

        public DateTime EklenmeTarihi { get; set; } = DateTime.Now;
    }
}
