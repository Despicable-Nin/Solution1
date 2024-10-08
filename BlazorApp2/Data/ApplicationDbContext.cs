using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp2.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<FlatCluster> FatClusters { get; set; }
        public DbSet<Crime> Crimes { get; set; }
        public DbSet<CrimeMotive> CrimeMotives { get; set; }
        public DbSet<Weather> WeatherConditions { get; set; } 
        public DbSet<PoliceDistrict> PoliceDistricts { get; set; }
        public DbSet<Severity> Severity { get; set; }
        public DbSet<CrimeType> CrimeTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Crime>()
                .HasKey(c => c.Id);
            builder.Entity<Crime>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();
            builder.Entity<Crime>().Property(c => c.DateUploaded).HasDefaultValue(DateTime.Now);

            builder.Entity<CrimeMotive>().HasKey(c => c.Id);
            builder.Entity<CrimeMotive>().Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Entity<CrimeMotive>().Property(c => c.Title).IsRequired();
            builder.Entity<CrimeMotive>().HasIndex(c => c.Title).IsUnique();

            builder.Entity<Weather>().HasKey(c => c.Id);
            builder.Entity<Weather>().Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Entity<Weather>().Property(c => c.Title).IsRequired();
            builder.Entity<Weather>().HasIndex(c => c.Title).IsUnique();

            builder.Entity<PoliceDistrict>().HasKey(c => c.Id);
            builder.Entity<PoliceDistrict>().Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Entity<PoliceDistrict>().Property(c => c.Title).IsRequired();
            builder.Entity<PoliceDistrict>().HasIndex(c => c.Title).IsUnique();

            builder.Entity<Severity>().HasKey(c => c.Id);
            builder.Entity<Severity>().Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Entity<Severity>().Property(c => c.Title).IsRequired();
            builder.Entity<Severity>().HasIndex(c => c.Title).IsUnique();

            builder.Entity<CrimeType>().HasKey(c => c.Id);
            builder.Entity<CrimeType>().Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Entity<CrimeType>().Property(c => c.Title).IsRequired();

            builder.Entity<FlatCluster>().HasKey(c => c.Id);
            builder.Entity<FlatCluster>().Property(c => c.Id).IsRequired();
            builder.Entity<FlatCluster>().Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Entity<FlatCluster>().HasIndex(c => c.CaseID).IsUnique();

            

        }

     
    }

}
