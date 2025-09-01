using LMS.Application.Services;
using LMS.Domain.Interfaces;
using LMS.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LMS.Domain.DTOs;

namespace LMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.Authenticate(request.Email, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // Map DepartmentID to departmentId for frontend
            return Ok(new
            {
                emailAddress = user.EmailAddress,
                userName = user.UserName,
                DepartmentID = user.DepartmentID,
                isActive = user.IsActive
            });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
