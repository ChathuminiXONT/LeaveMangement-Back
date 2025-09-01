using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Interfaces
{
    public interface ILeaveTypeRepository
    {
        Task<List<LeaveTypes>> GetActiveLeaveTypesAsync();
        Task<LeaveTypes?> GetLeaveTypeByIdAsync(string leaveType);
    }
}
