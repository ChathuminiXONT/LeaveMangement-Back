using LMS.Domain.DTOs;
using LMS.Domain.Models;

namespace LMS.Domain.Interfaces
{
    public interface ILeaveEntryRepository
    {
        Task<ApiResponse<long>> ApplyLeaveAsync(LeaveApplicationDto leaveApplication);
        Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetEmployeeLeaveHistoryAsync(string employeeNo, int year);
        Task<ApiResponse<LeaveApplicationResponseDto>> GetLeaveByIdAsync(long recId);
        Task<ApiResponse<List<string>>> GetApproverEmailsByUserEmailAsync(string email);

        Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetLeavesByEmailAsync(string email);
        Task<ApiResponse<List<LeaveBalanceDto>>> GetAvailableLeavesByEmailAndYearAsync(string email, int year);
        Task<ApiResponse<bool>> UpdateLeaveAsync(long recId, LeaveApplicationDto leaveApplication);
        Task<ApiResponse<bool>> DeleteLeaveAsync(long recId);
        Task<ApiResponse<bool>> ApproveRejectLeaveAsync(LeaveApprovalDto approval);
        Task<ApiResponse<List<LeaveBalanceDto>>> GetEmployeeLeaveBalanceAsync(string employeeNo, int year);
        Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetPendingApprovalsAsync(string approverEmail);
        Task<bool> CheckLeaveEntitlementAsync(string employeeNo, string leaveType, int leaveDays, int year);
        Task<string?> GetApproverEmailAsync(string employeeNo);
    }

  

   
}