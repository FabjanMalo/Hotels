﻿using AutoMapper;
using HotelListing.Models;
using Hotels.Data;

namespace HotelListing.Configurations;

public class MapperInitilizer : Profile
{
    public MapperInitilizer()
    {
        CreateMap<Country,CountryDTO>().ReverseMap();
        CreateMap<Country,CreateCountryDTO>().ReverseMap();
        CreateMap<Hotel,HotelDTO>().ReverseMap();
        CreateMap<Hotel,CreateHotelDTO >().ReverseMap();
    }
}