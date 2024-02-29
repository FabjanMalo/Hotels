
using HotelListing.Configurations.Entities;
using HotelListing.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Data;

public class DatabaseContext : IdentityDbContext<ApiUser>
{
    public DatabaseContext(DbContextOptions options) : base(options)
    { }
    DbSet<Country> Countries { get; set; }
    DbSet<Hotel> Hotels { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new CountryConfiguration());
        builder.ApplyConfiguration(new HotelConfiguration());
        builder.ApplyConfiguration(new RoleConfiguration());

        
    }
}
