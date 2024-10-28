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

    [HttpGet]
    public ActionResult<IEnumerable<HotelDto>> GetHotels() {
        return Ok(_repository.GetHotels());
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<HotelDto> PostHotel([FromBody] HotelInsertDto hotel) {
        try {
            return Created("", _repository.AddHotel(hotel));
        }
        catch (CityNotFoundException ex ) { return NotFound(new { ex.Message }); }
        catch (HotelAlreadyExistsException ex) { return Conflict(new { ex.Message }); }
    }
}