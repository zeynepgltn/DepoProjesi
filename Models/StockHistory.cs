using System;

namespace DepoProjesi.Models
{
    public class StockHistory
    {
        public int Id { get; set; }
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public int OncekiStok { get; set; }
        public int YeniStok { get; set; }
        public DateTime GuncellenmeTarihi { get; set; }
        public string Kullanici { get; set; }
    }
}
