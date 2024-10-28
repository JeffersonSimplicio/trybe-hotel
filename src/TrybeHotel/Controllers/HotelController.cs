using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Controllers;

[ApiController]
[Route("hotel")]
public class HotelController : Controller {
    private readonly IHotelRepository _repository;

    public HotelController(IHotelRepository repository) {
        _repository = repository;
    }

    [HttpGet("city/{cityId}")]
    public ActionResult<IEnumerable<HotelDto>> GetHotelsByCity(int cityId) {
        try {
            return Ok(_repository.GetHotelsByCity(cityId));
        }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpGet("search/{nameFragment}")]
    public ActionResult<IEnumerable<HotelDto>> GetHotelsByName(string nameFragment) {
        return Ok(_repository.GetHotelsByName(nameFragment));
    }

    [HttpGet]
    public ActionResult<IEnumerable<HotelDto>> GetAllHotels() {
        return Ok(_repository.GetAllHotels());
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<HotelDto> PostHotel([FromBody] HotelInsertDto hotel) {
        try {
            return Created("", _repository.AddHotel(hotel));
        }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
        catch (HotelAlreadyExistsException ex) { return Conflict(new { ex.Message }); }
    }
}