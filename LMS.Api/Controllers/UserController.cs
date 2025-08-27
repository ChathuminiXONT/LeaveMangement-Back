using LMS.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

       
        [HttpGet("by-email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery] string email)
        {
            try
            {
                var result = await _userService.GetUserByEmailAsync(email);
                if (result == null)
                    return BadRequest("Email is required.");
                

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Optionally log the exception here
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        [HttpGet("approver-details")]
        public async Task<IActionResult> GetUserApproverDetails([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            var result = await _userService.GetUserApproverDetailsAsync(email);
            return Ok(result);
        }

    }
}
