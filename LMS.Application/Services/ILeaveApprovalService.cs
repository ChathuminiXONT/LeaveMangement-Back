using LMS.Domain.DTOs;
using LMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Services
{
    public interface ILeaveApprovalService
    {
        
        Task<List<PendingLeaveRequestDto>> GetLeaveDetailsByDepartmentAsync(string departmentId);
        Task<ApprovalResponseDto> ApproveLeaveRequestAsync(ApprovalRequestDto request);
        Task<ApprovalResponseDto> RejectLeaveRequestAsync(ApprovalRequestDto request);
    }
}
