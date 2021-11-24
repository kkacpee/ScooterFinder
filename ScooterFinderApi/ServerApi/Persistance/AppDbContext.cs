using Microsoft.EntityFrameworkCore;
using ServerApi.Persistance.Models;

namespace ServerApi.Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Scooter> Scooters { get; set; }
    }
}
