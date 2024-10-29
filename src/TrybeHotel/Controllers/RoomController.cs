using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Controllers;

[ApiController]
[Route("room")]
public class RoomController : Controller {
    private readonly IRoomRepository _repository;
    public RoomController(IRoomRepository repository) {
        _repository = repository;
    }

    [HttpGet("{HotelId}")]
    public IActionResult GetRoom(int HotelId) {
        return Ok(_repository.GetRooms(HotelId));
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult PostRoom([FromBody] RoomInsertDto room) {
        try {
            return Created("", _repository.AddRoom(room));
        }
        catch (HotelNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpDelete("{RoomId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult Delete(int RoomId) {
        _repository.DeleteRoom(RoomId);
        return NoContent();
    }
}