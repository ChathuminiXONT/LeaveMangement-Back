//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using LMS.Domain.Models;
//using LMS.Domain.DTOs;

//namespace LMS.Domain.Interfaces
//{
//    public interface ILeaveApprovalRepository
//    {
//        Task<List<PendingLeaveRequestDto>> GetPendingLeaveRequestsByDepartmentAsync(String departmentId);
//        Task<int> GetPendingRequestsCountByDepartmentAsync(string departmentId);
//        Task<User> GetUserByEmailAsync(string email);
//        Task<LeaveDetails> GetLeaveRequestByIdAsync(int leaveRequestId);
//        Task<LeaveEntitle> GetLeaveEntitlementAsync(string empNo, string leaveType);
//        Task<bool> UpdateLeaveRequestStatusAsync(int leaveRequestId, int status, string comment, string approvedBy);
//        Task<bool> UpdateLeaveEntitlementAsync(LeaveEntitle leaveEntitlement);
//        Task<Departments> GetDepartmentByIdAsync(string departmentId);
//        Task<bool> IsUserApproverAsync(string email, string departmentId);
//    }
//}
