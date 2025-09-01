using System.Threading;
using System.Threading.Tasks;
using LMS.Domain.DTOs;


namespace LMS.Domain.Interfaces
{
    public interface ILeaveService
    {
        Task<ApiResponse<long>> ApplyLeaveAsync(LeaveApplicationDto leaveApplication);
        Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetEmployeeLeaveHistoryAsync(string employeeNo, int year);
        Task<ApiResponse<LeaveApplicationResponseDto>> GetLeaveByIdAsync(long recId);
        Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetLeavesByEmailAsync(string email); //new

        Task<ApiResponse<List<LeaveBalanceDto>>> GetAvailableLeavesByEmailAndYearAsync(string email, int year);

        Task<ApiResponse<bool>> UpdateLeaveAsync(long recId, LeaveApplicationDto leaveApplication);
        Task<ApiResponse<bool>> DeleteLeaveAsync(long recId);
        Task<ApiResponse<bool>> ApproveRejectLeaveAsync(LeaveApprovalDto approval);
        Task<ApiResponse<List<LeaveBalanceDto>>> GetEmployeeLeaveBalanceAsync(string employeeNo, int year);
        Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetPendingApprovalsAsync(string approverEmail);
    }
}
