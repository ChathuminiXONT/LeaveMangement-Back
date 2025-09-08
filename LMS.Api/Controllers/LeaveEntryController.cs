using LMS.Application.Services;
using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using LMS.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveEntryController : ControllerBase
    {
        private readonly ILeaveService _leaveService;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
   
        public LeaveEntryController(ILeaveService leaveService, ILeaveTypeRepository leaveTypeRepository)
        {
            _leaveService = leaveService;
            _leaveTypeRepository = leaveTypeRepository;
        }
        [HttpGet("approvers/{email}")]
        public async Task<IActionResult> GetApproversByUserEmail(string email)
        {
            var result = await _leaveService.GetApproverEmailsByUserEmailAsync(email);

            if (result.Success)
                return Ok(new { approverEmails = result.Data }); // Data holds List<string>

            return NotFound(new { error = result.Message }); // Message holds error info
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveApplicationDto leaveApplication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _leaveService.ApplyLeaveAsync(leaveApplication);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("by-email/{email}")]         //new
        public async Task<IActionResult> GetLeavesByEmail(string email)
        {
            var result = await _leaveService.GetLeavesByEmailAsync(email);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }
        [HttpGet("available-leaves")]
        public async Task<IActionResult> GetAvailableLeaves([FromQuery] string email, [FromQuery] int year)
        {
            var result = await _leaveService.GetAvailableLeavesByEmailAndYearAsync(email, year);
            if (result.Success) return Ok(result);
            return NotFound(result);
        }


        [HttpGet("history/{employeeNo}/{year}")]
        public async Task<IActionResult> GetLeaveHistory(string employeeNo, int year)
        {
            var result = await _leaveService.GetEmployeeLeaveHistoryAsync(employeeNo, year);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("{recId}")]
        public async Task<IActionResult> GetLeaveById(long recId)
        {
            var result = await _leaveService.GetLeaveByIdAsync(recId);

            if (result.Success)
                return Ok(result);

            return NotFound(result);
        }

        [HttpPut("{recId}")]
        public async Task<IActionResult> UpdateLeave(long recId, [FromBody] LeaveApplicationDto leaveApplication)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _leaveService.UpdateLeaveAsync(recId, leaveApplication);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{recId}")]
        public async Task<IActionResult> DeleteLeave(long recId)
        {
            var result = await _leaveService.DeleteLeaveAsync(recId);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("approve-reject")]
        public async Task<IActionResult> ApproveRejectLeave([FromBody] LeaveApprovalDto approval)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _leaveService.ApproveRejectLeaveAsync(approval);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("balance/{employeeNo}/{year}")]
        public async Task<IActionResult> GetLeaveBalance(string employeeNo, int year)
        {
            var result = await _leaveService.GetEmployeeLeaveBalanceAsync(employeeNo, year);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("pending-approvals/{approverEmail}")]
        public async Task<IActionResult> GetPendingApprovals(string approverEmail)
        {
            var result = await _leaveService.GetPendingApprovalsAsync(approverEmail);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("leave-types")]
        public async Task<IActionResult> GetLeaveTypes()
        {
            var leaveTypes = await _leaveTypeRepository.GetActiveLeaveTypesAsync();
            return Ok(new ApiResponse<List<LeaveTypes>>
            {
                Success = true,
                Data = leaveTypes
            });
        }
    }
}