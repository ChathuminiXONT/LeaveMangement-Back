using LMS.Domain.Models;
using LMS.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.DTOs;

namespace LMS.Application.Services
{
    public class LeaveApprovalService: ILeaveApprovalService
    {
        private readonly ILeaveApprovalRepository _leaveApprovalRepository;
        public LeaveApprovalService(ILeaveApprovalRepository leaveApprovalRepository)
        {
            _leaveApprovalRepository = leaveApprovalRepository;
        }
            public async Task<List<PendingLeaveRequestDto>> GetLeaveDetailsByDepartmentAsync(string departmentId)
            {
                try
                {
                    return await _leaveApprovalRepository.GetLeaveDetailsByDepartmentAsync(departmentId);
            }
                catch (Exception ex)
                {
                   
                    throw new ApplicationException($"Error retrieving leave details for department {departmentId}: {ex.Message}", ex);
                }
            }
        public async Task<ApprovalResponseDto> ApproveLeaveRequestAsync(ApprovalRequestDto request)
        {
            try
            {
                return await _leaveApprovalRepository.ApproveLeaveRequestAsync(request);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error approving leave request {request.RecId}: {ex.Message}", ex);
            }
        }
        public async Task<ApprovalResponseDto> RejectLeaveRequestAsync(ApprovalRequestDto request)
        {
            try
            {
                return await _leaveApprovalRepository.RejectLeaveRequestAsync(request);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error rejecting leave request {request.RecId}: {ex.Message}", ex);
            }
        }
    }
}
