using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;
using TrybeHotel.Exceptions;

namespace TrybeHotel.Controllers;

[ApiController]
[Route("booking")]

public class BookingController : Controller {
    private readonly IBookingRepository _repository;
    public BookingController(IBookingRepository repository) {
        _repository = repository;
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Client")]
    public ActionResult<BookingResponse> AddBooking([FromBody] BookingDtoInsert bookingInsert) {
        try {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var userId = int.Parse(
                token!.Claims.SingleOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier
                )!.Value
            );
            BookingResponse bookindDto = _repository.AddBooking(bookingInsert, userId);
            return CreatedAtAction(
                nameof(GetBooking),
                new { bookingId = bookindDto.BookingId },
                bookindDto
            );
        }
        catch (RoomNotFoundException ex) { return NotFound(new { messager = ex.Message }); }
        catch (RoomCapacityExceededException ex) {
            return BadRequest(new { messager = ex.Message });
        }
        catch (InvalidBookingDateException ex) {
            return BadRequest(new { messager = ex.Message });
        }
        catch (RoomUnavailableException ex) {
            return Conflict(new { messager = ex.Message });
        }
    }

    [HttpGet("{bookingId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<BookingResponse> GetBooking(int bookingId) {
        try {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            int userId = int.Parse(
                token!.Claims.FirstOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier
                )!.Value
            );
            string userType = token!.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)!
                .Value;
            return Ok(_repository.GetBookingById(bookingId, userId, userType));
        }
        catch (BookingNotFoundException ex) { return NotFound(new { messager = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    [HttpPut("{bookingId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Client")]
    public ActionResult<BookingResponse> UpdateBooking(
        int bookingId,
        [FromBody] BookingDtoInsert bookingInsert
    ) {
        try {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var userId = int.Parse(
                token!.Claims.SingleOrDefault(
                    c => c.Type == ClaimTypes.NameIdentifier
                )!.Value
            );
            BookingResponse bookindDto = _repository.UpdateBooking(
                bookingId,
                bookingInsert,
                userId
            );
            return CreatedAtAction(
                nameof(GetBooking),
                new { bookingId = bookindDto.BookingId },
                bookindDto
            );
        }
        catch (RoomNotFoundException ex) { return NotFound(new { messager = ex.Message }); }
        catch (RoomCapacityExceededException ex) {
            return BadRequest(new { messager = ex.Message });
        }
        catch (InvalidBookingDateException ex) {
            return BadRequest(new { messager = ex.Message });
        }
        catch (RoomUnavailableException ex) {
            return Conflict(new { messager = ex.Message });
        }
    }

    [HttpDelete("{bookingId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Client")]
    public ActionResult<BookingResponse> DeleteBooking(int bookingId) {
        var token = HttpContext.User.Identity as ClaimsIdentity;
        var userId = int.Parse(
            token!.Claims.SingleOrDefault(
                c => c.Type == ClaimTypes.NameIdentifier
            )!.Value
        );
        try {
            _repository.DeleteBooking(bookingId, userId);
            return NoContent();
        }
        catch (BookingNotFoundException ex) { return NotFound(new { messager = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException ex) { return BadRequest(new { messager = ex.Message }); }
    }
}