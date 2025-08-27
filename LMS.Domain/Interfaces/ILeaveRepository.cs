using LMS.Domain.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Domain.Interfaces
{
    public interface ILeaveRepository
    {
        Task<IEnumerable<LeaveDto>> GetLeavesByMonthAsync(int year, int month);
    }
}
