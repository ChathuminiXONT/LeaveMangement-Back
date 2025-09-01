using LMS.Application.Services;
using LMS.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LMS.Domain.Interfaces;

namespace LMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveController : ControllerBase
    {
        private readonly LeaveService _leaveService;

        public LeaveController(LeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpGet("month/{year:int}/{month:int}")]
        public async Task<ActionResult<IEnumerable<LeaveDto>>> GetLeavesByMonth(int year, int month)
        {
            try
            {
                var leaves = await _leaveService.GetLeavesByMonthAsync(year, month);
                return Ok(leaves);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
