using espasyo_domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace espasyo_domain
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Incident> Incidents { get; set; }
    }
}
