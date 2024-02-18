using Microsoft.EntityFrameworkCore;

namespace Hotels.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options)
    { }
    DbSet<Country> Countries { get; set; }
    DbSet<Hotel> Hotels { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Country>().HasData(
            new Country
            {
                Id = 1,
                Name = "Jamaica",
                ShortName = "JM",
            },
            new Country
            {
                Id = 2,
                Name = "Albania",
                ShortName = "Alb",

            }, new Country
            {
                Id = 3,
                Name = "Germany",
                ShortName = "Ger",
            }
            );
        builder.Entity<Hotel>().HasData(
           new Hotel
           {
               Id = 1,
               Name = "JM Spa",
               Addres = "Negril",
               Rating = 4.5,
               CountryId = 1,
           },
           new Hotel
           {
               Id = 2,

               Name = "Marin Hotel",
               Addres = "Trana",
               Rating = 5.0,
               CountryId = 2,

           }, new Hotel
           {
               Id = 3,
               Name = "German Resort",
               Addres = "Mynih",
               Rating = 3.5,
               CountryId = 3,
           }
           );
    }
}
