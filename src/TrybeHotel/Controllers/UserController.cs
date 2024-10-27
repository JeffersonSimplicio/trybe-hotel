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
    public ActionResult<IEnumerable<UserDto>> GetAllUsers() {
        return Ok(_repository.GetAllUsers());
    }


    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult<IEnumerable<UserDto>> GetUsersByName([FromBody] string userName) {
        return Ok(_repository.GetUsersByName(userName));
    }

    [HttpPost]
    public ActionResult<UserDto> AddUser([FromBody] UserDtoInsert user) {
        try {
            return Created("", _repository.AddUser(user));
        }
        catch (EmailAlreadyExistsException ex) {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<UserDto> UpdateUser([FromBody] UserDtoUpdate userUpdate) {
        try {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            string userType = token!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value;
            UserDto user = _repository.UpdateUser(userUpdate, userType);
            return Ok(user);
        }
        catch (UserNotFoundException ex) { return NotFound(ex.Message); }
    }

    [HttpDelete]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult DeleteOwnAccount() {
        try {
            var token = HttpContext.User.Identity as ClaimsIdentity;
            string userEmail = token!.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

            _repository.DeleteOwnAccount(userEmail);
            return NoContent();
        }
        catch (UserNotFoundException ex) { return NotFound(ex.Message); }
    }

    [HttpDelete("{UserId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = "Admin")]
    public ActionResult AdminDeleteUser(int UserId) {
        try {
            _repository.AdminDeleteUser(UserId);
            return NoContent();
        }
        catch (UserNotFoundException ex) { return NotFound(ex.Message); }
    }
}
