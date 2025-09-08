using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Services
{
    public class LeaveEntryService : ILeaveService
    {
        private readonly ILeaveEntryRepository _leaveRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILogger<LeaveEntryService> _logger;

        public LeaveEntryService(
            ILeaveEntryRepository leaveRepository,
            ILeaveTypeRepository leaveTypeRepository,
            ILogger<LeaveEntryService> logger)
        {
            _leaveRepository = leaveRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<long>> ApplyLeaveAsync(LeaveApplicationDto leaveApplication)
        {
            try
            {
                var validationResult = await ValidateLeaveApplicationAsync(leaveApplication);
                if (!validationResult.Success)
                {
                    return new ApiResponse<long>
                    {
                        Success = false,
                        Message = validationResult.Message
                    };
                }

                return await _leaveRepository.ApplyLeaveAsync(leaveApplication);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LeaveService.ApplyLeaveAsync");
                return new ApiResponse<long>
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }
        //new
        public async Task<ApiResponse<List<string>>> GetApproverEmailsByUserEmailAsync(string email)
        {
            try
            {
                // Delegate to repository
                return await _leaveRepository.GetApproverEmailsByUserEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LeaveEntryService.GetApproverEmailsByUserEmailAsync");
                return new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = "An error occurred while fetching approver emails."
                };
            }
        }

        public async Task<ApiResponse<List<LeaveBalanceDto>>> GetAvailableLeavesByEmailAndYearAsync(string email, int year)
        {
            try { return await _leaveRepository.GetAvailableLeavesByEmailAndYearAsync(email, year); }
            catch (Exception ex) { _logger.LogError(ex, "Error in LeaveService..."); return new ApiResponse<List<LeaveBalanceDto>> { Success = false, Message = "An error occurred." }; }
        }

        public async Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetEmployeeLeaveHistoryAsync(string employeeNo, int year)
        {
            return await _leaveRepository.GetEmployeeLeaveHistoryAsync(employeeNo, year);
        }

        public async Task<ApiResponse<LeaveApplicationResponseDto>> GetLeaveByIdAsync(long recId)
        {
            return await _leaveRepository.GetLeaveByIdAsync(recId);
        }
        public async Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetLeavesByEmailAsync(string email) //new
        {
            try
            {
                return await _leaveRepository.GetLeavesByEmailAsync(email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LeaveService.GetLeavesByEmailAsync");
                return new ApiResponse<List<LeaveApplicationResponseDto>>
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }

        public async Task<ApiResponse<bool>> UpdateLeaveAsync(long recId, LeaveApplicationDto leaveApplication)
        {
            try
            {
                var validationResult = await ValidateLeaveApplicationAsync(leaveApplication);
                if (!validationResult.Success)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = validationResult.Message
                    };
                }

                return await _leaveRepository.UpdateLeaveAsync(recId, leaveApplication);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LeaveService.UpdateLeaveAsync");
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteLeaveAsync(long recId)
        {
            return await _leaveRepository.DeleteLeaveAsync(recId);
        }

        public async Task<ApiResponse<bool>> ApproveRejectLeaveAsync(LeaveApprovalDto approval)
        {
            if (approval.LeaveStatus != "1" && approval.LeaveStatus != "3")
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Invalid leave status. Use '1' for approved or '3' for rejected."
                };
            }

            return await _leaveRepository.ApproveRejectLeaveAsync(approval);
        }

        public async Task<ApiResponse<List<LeaveBalanceDto>>> GetEmployeeLeaveBalanceAsync(string employeeNo, int year)
        {
            return await _leaveRepository.GetEmployeeLeaveBalanceAsync(employeeNo, year);
        }

        public async Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetPendingApprovalsAsync(string approverEmail)
        {
            return await _leaveRepository.GetPendingApprovalsAsync(approverEmail);
        }

        private async Task<ApiResponse<object>> ValidateLeaveApplicationAsync(LeaveApplicationDto leaveApplication)
        {
            var leaveType = await _leaveTypeRepository.GetLeaveTypeByIdAsync(leaveApplication.LeaveType);
            if (leaveType == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Invalid leave type."
                };
            }

            if (leaveApplication.LeaveStart.Date < DateTime.Today)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Leave start date cannot be in the past."
                };
            }

            if (leaveApplication.LeaveEnd < leaveApplication.LeaveStart)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Leave end date must be after start date."
                };
            }

            //var daysDifference = (leaveApplication.LeaveEnd - leaveApplication.LeaveStart).Days + 1;
            //if (leaveApplication.LeaveDays != daysDifference)
            //{
            //    return new ApiResponse<object>
            //    {
            //        Success = false,
            //        Message = "Leave days calculation mismatch."
            //    };
            //}

            return new ApiResponse<object> { Success = true };
        }
    }
}
