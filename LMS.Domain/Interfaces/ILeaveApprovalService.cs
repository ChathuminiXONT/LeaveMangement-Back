//using LMS.Domain.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LMS.Domain.Interfaces
//{
//    public interface ILeaveApprovalService
//    {
//        Task<List<PendingLeaveRequestDto>> GetPendingLeaveRequestsAsync(string approverEmail);
//        Task<PendingRequestsCountDto> GetPendingRequestsCountAsync(string approverEmail);
//        Task<ApprovalResponseDto> ProcessLeaveApprovalAsync(ApprovalRequestDto request);
//        Task<bool> ValidateApproverPermissionsAsync(string approverEmail, int leaveRequestId);
//    }
//}
