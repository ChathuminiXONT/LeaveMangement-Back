using LMS.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendLeaveApplicationEmailAsync(string approverEmail, LeaveApplicationResponseDto leaveDetails);
        Task<bool> SendLeaveStatusEmailAsync(string employeeEmail, LeaveApplicationResponseDto leaveDetails, bool isApproved);
    }
}


