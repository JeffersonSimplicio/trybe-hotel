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
    public ActionResult<CityDto> GetCityById(int CityId) {
        try { return Ok(_repository.GetCityById(CityId)); }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetAllCities() {
        return Ok(_repository.GetAllCities());
    }

    [HttpGet("search/{nameFragment}")]
    public ActionResult<IEnumerable<CityDto>> FindCitiesByName(string nameFragment) {
        return Ok(_repository.FindCitiesByName(nameFragment));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<CityDto> AddCity([FromBody] CityDtoInsert city) {
        try {
            CityDto newCity = _repository.AddCity(city);
            return CreatedAtAction(nameof(GetCityById), new { CityId = newCity.cityId }, newCity);
        }
        catch (CityAlreadyExistsException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<CityDto> UpdateCity([FromBody] CityDto city) {
        try { return Ok(_repository.UpdateCity(city)); }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpDelete("{CityId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult DeleteCity(int CityId) {
        try {
            _repository.DeleteCity(CityId);
            return NoContent();
        }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }
}