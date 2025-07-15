using System.ComponentModel.DataAnnotations;

namespace DepoProjesi.Models
{
    public class Employee
    {
        [Key]
        public int PersonelId { get; set; } 

        public required string Ad { get; set; }
        public required string Soyad { get; set; }
        public required string Departman { get; set; }
        public required string Email { get; set; }
        public required string Sifre { get; set; }

        public required string Rol { get; set; }
    }
}
