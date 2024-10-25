using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TrybeHotel.Exceptions;
using System.Security.Claims;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers;

[ApiController]
[Route("user")]
public class UserController : Controller {
    private readonly IUserRepository _repository;
    public UserController(IUserRepository repository) {
        _repository = repository;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public IActionResult GetUsers() {
        return Ok(_repository.GetUsers());
    }

    [HttpPost]
    public IActionResult Add([FromBody] UserDtoInsert user) {
        try {
            return Created("", _repository.Add(user));
        }
        catch (Exception ex) {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult Update([FromBody] UserDtoUpdate userUpdate) {
        try {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            string userType = token!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
            UserDto user = _repository.Update(userUpdate, userType);
            return Ok(new { token = new TokenGenerator().Generate(user) });
        }
        catch (UserNotFoundException ex) { return NotFound(ex.Message); }
    }
}
