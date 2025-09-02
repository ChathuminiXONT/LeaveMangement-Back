using LMS.Application.Services;
using Microsoft.AspNetCore.Mvc;
using LMS.Domain.DTOs;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecentActivityController : ControllerBase
    {
        private readonly RecentActivityService _recentActivityService;

        public RecentActivityController(RecentActivityService recentActivityService)
        {
            _recentActivityService = recentActivityService;
        }

        // GET: api/RecentActivity?email=user@email.com
        [HttpGet]
        public async Task<IActionResult> GetRecentActivities([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required");

            var activities = await _recentActivityService.GetRecentActivitiesAsync(email);
            return Ok(activities);
        }
    }
}
