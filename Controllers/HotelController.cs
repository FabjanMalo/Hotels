using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Hotels.Data;
using Microsoft.AspNetCore.Authorization;
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


     [HttpGet("{id:int}", Name ="GetHotel")]
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

//[Authorize(Roles ="Administrator")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Attempt in {nameof(CreateHotel)}");
            return BadRequest(ModelState);
        }
        try
        {
            var hotel = _mapper.Map<Hotel>(hotelDTO);
            await _unitOfWork.Hotels.Insert(hotel);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetHotel", new { id = hotel.Id },hotel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(CreateHotel)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }

   //[Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> UpdateHotel(int id,[FromBody] UpdateHotelDTO hotelDTO)
    {
        if (!ModelState.IsValid || id<1)
        {
            _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateHotel)}");
            return BadRequest(ModelState);
        }
           
        try
        {
            var hotel = await _unitOfWork.Hotels.Get(q=> q.Id == id);
            if(hotel == null)
            {
                _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateHotel)}");
                return BadRequest("Submitted Data Is Valid");
            }

          hotel = _mapper.Map(hotelDTO, hotel);
             _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateHotel)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> DeleteHotel(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE Attempt in {nameof(DeleteHotel)}");
            return BadRequest();
        }

        try
        {
            var hotel = await _unitOfWork.Countries.Get(q => q.Id == id);
            if (hotel == null)
            {
                _logger.LogError($"Invalid UPDATE Attempt in {nameof(DeleteHotel)}");
                return BadRequest("Submitted Data Is Valid");
            }

            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteHotel)}");
            return StatusCode(500, "Internal Server Error. Please Try Again Later.");
        }
    }

}
