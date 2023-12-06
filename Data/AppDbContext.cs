using Microsoft.EntityFrameworkCore;
using Negotiator.Models;

namespace Negotiator.Data
{
public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Negotiation> Negotiations { get; set; }
    }

}

