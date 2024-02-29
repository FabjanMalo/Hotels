using Hotels.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Configurations.Entities;

public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.HasData(
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
