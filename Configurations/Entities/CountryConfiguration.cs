using Hotels.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configurations.Entities;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
       builder.HasData(
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
    }
}
