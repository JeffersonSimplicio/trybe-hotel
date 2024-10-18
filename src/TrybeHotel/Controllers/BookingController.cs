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
    public IActionResult Add([FromBody] BookingDtoInsert bookingInsert) {
        try {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            BookingResponse bookindDto = _repository.Add(bookingInsert, email);
            return Created("", bookindDto);
        }
        catch (Exception ex) {
            return BadRequest(new { messager = ex.Message });
        }
    }

    [HttpGet("{Bookingid}")]
    public IActionResult GetBooking(int Bookingid) {
        try {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            var email = token?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            BookingResponse bookindDto = _repository.GetBooking(Bookingid, email);
            return Ok(bookindDto);
        }
        catch (Exception ex) {
            return Unauthorized(new { messager = ex.Message });
        }
    }
}