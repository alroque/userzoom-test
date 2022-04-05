using Microsoft.EntityFrameworkCore;

namespace Core.Persistence
{
    public class UserZoomContext : DbContext
    {
        public UserZoomContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Orders> Orders { get; set; }
    }
}