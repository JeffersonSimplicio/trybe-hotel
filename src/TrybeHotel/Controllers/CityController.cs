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

    [HttpGet("{cityId}/hotel")]
    public ActionResult<CityDto> FindHotelByCity(int cityId) {
        try { return Ok(_repository.FindHotelsByCity(cityId)); }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpGet("{cityId}")]
    public ActionResult<CityDto> GetCityById(int cityId) {
        try { return Ok(_repository.GetCityById(cityId)); }
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
            return CreatedAtAction(nameof(GetCityById), new { cityId = newCity.CityId }, newCity);
        }
        catch (CityAlreadyExistsException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpPut("{cityId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<CityDto> UpdateCity(int cityId, [FromBody] CityDtoInsert city) {
        try { return Ok(_repository.UpdateCity(cityId, city)); }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
        catch (CityAlreadyExistsException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpDelete("{cityId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult DeleteCity(int cityId) {
        try {
            _repository.DeleteCity(cityId);
            return NoContent();
        }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }
}