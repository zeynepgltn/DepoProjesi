using Microsoft.EntityFrameworkCore;
using DepoProjesi.Models;

namespace DepoProjesi.Data
{
    public class WarehouseDbContext : DbContext
    {
        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }

        public DbSet<Product> Urunler { get; set; }
        public DbSet<Employee> Personeller { get; set; }
    }
}
