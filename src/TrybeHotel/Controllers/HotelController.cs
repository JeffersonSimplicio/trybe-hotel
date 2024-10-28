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

    [HttpGet("search/{nameFragment}")]
    public ActionResult<IEnumerable<HotelDto>> GetHotelsByName(string nameFragment) {
        return Ok(_repository.GetHotelsByName(nameFragment));
    }

    [HttpGet]
    public ActionResult<IEnumerable<HotelDto>> GetAllHotels() {
        return Ok(_repository.GetAllHotels());
    }

    [HttpGet("{hotelId}")]
    public ActionResult<HotelDto> GetHotelsById(int hotelId) {
        try { return Ok(_repository.GetHotelById(hotelId)); }
        catch (HotelNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<HotelDto> AddHotel([FromBody] HotelInsertDto hotel) {
        try {
            return Created("", _repository.AddHotel(hotel));
        }
        catch (CityNotFoundException ex) { return NotFound(new { ex.Message }); }
        catch (HotelAlreadyExistsException ex) { return Conflict(new { ex.Message }); }
    }

    [HttpDelete("{hotelId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult DeleteHotel(int hotelId) {
        try {
            _repository.DeleteHotel(hotelId);
            return NoContent();
        }
        catch (HotelNotFoundException ex) { return NotFound(new { ex.Message }); }
    }
}