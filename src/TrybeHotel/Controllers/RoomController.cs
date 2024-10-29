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

    [HttpGet("{roomId}")]
    public IActionResult GetRoomById(int roomId) {
        try { return Ok(_repository.GetRoomById(roomId)); }
        catch (RoomNotFoundException ex) { return NotFound(new { ex.Message }); }
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult PostRoom([FromBody] RoomInsertDto room) {
        try {
            RoomDto newRoom = _repository.AddRoom(room);
            return CreatedAtAction(nameof(GetRoomById), new { roomId = newRoom.RoomId }, newRoom);
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