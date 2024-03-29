﻿using AutoMapper;
using HotelListing.IRepository;
using HotelListing.Models;
using Hotels.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Controllers;
[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CountryController> _logger;
    private readonly IMapper _mapper;

  
   public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
  
    [HttpGet]
    
    public async Task<IActionResult> GetCountries([FromQuery] RequestParams requestParams)
    {
        var countries = await _unitOfWork.Countries.GetPagedList(requestParams);
        var result = _mapper.Map<IList<CountryDTO>>(countries);
        return Ok(result);
    }

   
    [HttpGet("{id:int}", Name = "GetCountry")]
    public async Task<IActionResult> GetCountry(int id)
    {
        var country = await _unitOfWork.Countries.Get(co => co.Id == id, new List<string> { "Hotels" });
        var result = _mapper.Map<CountryDTO>(country);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDTO countryDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"Invalid POST Attempt in {nameof(CreateCountry)}");
            return BadRequest(ModelState);
        }

        var country = _mapper.Map<Country>(countryDTO);
        await _unitOfWork.Countries.Insert(country);
        await _unitOfWork.Save();

        return CreatedAtRoute("GetCountry", new { id = country.Id }, country);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDTO countryDTO)
    {
        if (!ModelState.IsValid || id < 1)
        {
            _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateCountry)}");
            return BadRequest(ModelState);
        }


        var country = await _unitOfWork.Countries.Get(q => q.Id == id);
        if (country == null)
        {
            _logger.LogError($"Invalid UPDATE Attempt in {nameof(UpdateCountry)}");
            return BadRequest("Submitted Data Is Valid");
        }

        country = _mapper.Map(countryDTO, country);
        _unitOfWork.Countries.Update(country);
        await _unitOfWork.Save();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> DeleteCountry(int id)
    {
        if (id < 1)
        {
            _logger.LogError($"Invalid DELETE Attempt in {nameof(DeleteCountry)}");
            return BadRequest();
        }

        var country = await _unitOfWork.Countries.Get(q => q.Id == id);
        if (country == null)
        {
            _logger.LogError($"Invalid UPDATE Attempt in {nameof(DeleteCountry)}");
            return BadRequest("Submitted Data Is Valid");
        }

        await _unitOfWork.Countries.Delete(id);
        await _unitOfWork.Save();

        return NoContent();

    }

}
