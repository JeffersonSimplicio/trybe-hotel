using Microsoft.AspNetCore.Mvc;
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
        catch (CityNotFoundException) { return NotFound(); }
    }

    [HttpGet]
    public IActionResult GetCities() {
        return Ok(_repository.GetCities());
    }

    [HttpGet("{nameFragment}")]
    public IActionResult GetCities(string nameFragment) {
        return Ok(_repository.GetCities(nameFragment));
    }

    [HttpPost]
    public IActionResult PostCity([FromBody] City city) {
        return Created("", _repository.AddCity(city));
    }

    [HttpPut]
    public IActionResult PutCity([FromBody] City city) {
        return Ok(_repository.UpdateCity(city));
    }

    [HttpDelete("{CityId}")]
    public IActionResult DeleteCity(int CityId) {
        try {
            _repository.DeleteCity(CityId);
            return NoContent();
        }
        catch (CityNotFoundException) { return NotFound(); }
    }
}