using LMS.Application.Services;
using LMS.Domain.DTOs;
using LMS.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveApprovalController : ControllerBase
    {
        private readonly ILeaveApprovalService _leaveApprovalService;

        public LeaveApprovalController(ILeaveApprovalService leaveApprovalService)
        {
            _leaveApprovalService = leaveApprovalService;
        }

        [HttpGet("pending-by-department")]
        public async Task<IActionResult> GetPendingLeaveDetailsByDepartment([FromQuery] string departmentId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(departmentId))
                    return BadRequest("Department ID is required.");

                var result = await _leaveApprovalService.GetLeaveDetailsByDepartmentAsync(departmentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
           
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("approve")]
        public async Task<IActionResult> ApproveLeaveRequest([FromBody] ApprovalRequestDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request data is required.");

                if (request.RecId <= 0)
                    return BadRequest("RecId is required.");

                if (string.IsNullOrWhiteSpace(request.EmployeeEmail))
                    return BadRequest("Employee email is required.");

                if (string.IsNullOrWhiteSpace(request.ApproverEmail))
                    return BadRequest("Approver email is required.");

                if (request.ApproverLevel != "Approver1" && request.ApproverLevel != "Approver2")  // Changed validation
                    return BadRequest("Approver level must be 'Approver1' or 'Approver2'.");

                if (request.LeaveDays <= 0)
                    return BadRequest("Leave days must be greater than 0.");

                var result = await _leaveApprovalService.ApproveLeaveRequestAsync(request);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        [HttpPost("reject")]
        public async Task<IActionResult> RejectLeaveRequest([FromBody] ApprovalRequestDto request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request data is required.");

                if (request.RecId <= 0)
                    return BadRequest("RecId is required.");

                if (string.IsNullOrWhiteSpace(request.EmployeeEmail))
                    return BadRequest("Employee email is required.");

                if (string.IsNullOrWhiteSpace(request.ApproverEmail))
                    return BadRequest("Approver email is required.");

                if (request.ApproverLevel != "Approver1" && request.ApproverLevel != "Approver2")
                    return BadRequest("Approver level must be 'Approver1' or 'Approver2'.");

                if (request.LeaveDays <= 0)
                    return BadRequest("Leave days must be greater than 0.");

                var result = await _leaveApprovalService.RejectLeaveRequestAsync(request);

                if (result.Success)
                    return Ok(result);
                else
                    return BadRequest(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}