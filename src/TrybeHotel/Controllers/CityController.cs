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
        catch (Exception) { return StatusCode(500); }
    }

    [HttpGet]
    public IActionResult GetCities() {
        return Ok(_repository.GetCities());
    }

    [HttpPost]
    public IActionResult PostCity([FromBody] City city) {
        return Created("", _repository.AddCity(city));
    }

    // 3. Desenvolva o endpoint PUT /city
    [HttpPut]
    public IActionResult PutCity([FromBody] City city) {
        return Ok(_repository.UpdateCity(city));
    }
}