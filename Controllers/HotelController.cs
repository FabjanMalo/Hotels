using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HotelController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HotelController> _logger;
    private readonly IMapper _mapper;


    public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IActionResult> GetHotels()
    {
        try
        {

            var hotels = await _unitOfWork.Hotels.GetAll();
            var result = _mapper.Map<IList<HotelDTO>>(hotels);
            return Ok(result);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }

     [HttpGet("{id:int}")]
    public async Task<IActionResult> GetHotels(int id)
    {
        try
        {

            var hotels = await _unitOfWork.Hotels.Get(co => co.Id == id, new List<string> { "Country" });
            var result = _mapper.Map<HotelDTO>(hotels);
            return Ok(result);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(GetHotels)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }
}
