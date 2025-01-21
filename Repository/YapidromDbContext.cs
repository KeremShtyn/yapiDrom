using Entity.Models;
using Microsoft.EntityFrameworkCore;


namespace Persistence
{
    public class YapidromDbContext : DbContext
    {
        public YapidromDbContext(DbContextOptions<YapidromDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Scooter> Scooters { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
    }
}
