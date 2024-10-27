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
    public ActionResult<CityDto> GetById(int CityId) {
        try { return Ok(_repository.GetById(CityId)); }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpGet]
    public ActionResult<IEnumerable<CityDto>> GetAll() {
        return Ok(_repository.GetAll());
    }

    [HttpGet("search/{nameFragment}")]
    public ActionResult<IEnumerable<CityDto>> FindByName(string nameFragment) {
        return Ok(_repository.FindByName(nameFragment));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<CityDto> Add([FromBody] CityDtoInsert city) {
        try {
            CityDto newCity = _repository.Add(city);
            return CreatedAtAction(nameof(GetById), new { CityId = newCity.cityId }, newCity);
        }
        catch (CityAlreadyExistsException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<CityDto> Update([FromBody] CityDto city) {
        try { return Ok(_repository.Update(city)); }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpDelete("{CityId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int CityId) {
        try {
            _repository.Delete(CityId);
            return NoContent();
        }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }
}