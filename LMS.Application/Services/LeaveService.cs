using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMS.Application.Services
{
    public class LeaveService
    {
        private readonly ILeaveRepository _leaveRepository;

        public LeaveService(ILeaveRepository leaveRepository)
        {
            _leaveRepository = leaveRepository;
        }

        public Task<IEnumerable<LeaveDto>> GetLeavesByMonthAsync(int year, int month)
        {
            return _leaveRepository.GetLeavesByMonthAsync(year, month);
        }
    }
}
