using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Hotels.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers;
[ApiVersion("2.0")]
[Route("api/{v:apiversion}/country")]
[ApiController]
public class CountryV2Controller : ControllerBase
{

    private DatabaseContext _dbContext;

    public CountryV2Controller(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        return Ok(_dbContext.Countries);
    }

}
