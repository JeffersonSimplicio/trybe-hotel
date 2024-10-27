using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;
using TrybeHotel.Models;
using TrybeHotel.Repository;

namespace TrybeHotel.Controllers;

[ApiController]
[Route("city")]
public class CityController : Controller {
    private readonly ICityRepository _repository;
    public CityController(ICityRepository repository) {
        _repository = repository;
    }

    [HttpGet("{CityId}")]
    public IActionResult GetCity(int CityId) {
        try { return Ok(_repository.GetCity(CityId)); }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpGet]
    public IActionResult GetCities() {
        return Ok(_repository.GetCities());
    }

    [HttpGet("search/{nameFragment}")]
    public IActionResult GetCities(string nameFragment) {
        return Ok(_repository.GetCities(nameFragment));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult PostCity([FromBody] CityDtoInsert city) {
        try {
            CityDto newCity = _repository.AddCity(city);
            return CreatedAtAction(nameof(GetCity), new { CityId = newCity.cityId }, newCity);
        }
        catch (CityAlreadyExistsException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult PutCity([FromBody] CityDto city) {
        try { return Ok(_repository.UpdateCity(city)); }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpDelete("{CityId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult DeleteCity(int CityId) {
        try {
            _repository.DeleteCity(CityId);
            return NoContent();
        }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }
}