using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly LMSDbContext _context;

        public LeaveRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveDto>> GetLeavesByMonthAsync(int year, int month)
        {
            var leaves = await (
                from l in _context.LeaveDetails
                join u in _context.XDUsers
                    on l.EmpEmailID.Trim().ToLower() equals u.EmailAddress.Trim().ToLower() into userJoin
                from user in userJoin.DefaultIfEmpty()
                where l.LeaveStart != null && l.LeaveStart.Value.Year == year && l.LeaveStart.Value.Month == month
                select new LeaveDto
                {
                    LeaveStart = l.LeaveStart ?? default, // fallback default if null
                    StartTime = l.StartTime.HasValue ? l.StartTime.Value.ToString(@"hh\:mm") : "00:00",
                    LeaveEnd = l.LeaveEnd ?? l.LeaveStart ?? default,
                    EndTime = l.EndTime.HasValue ? l.EndTime.Value.ToString(@"hh\:mm") : "00:00",
                    LeaveType = l.LeaveType ?? string.Empty,
                    UserName = user != null ? user.UserName ?? "Unknown" : "Unknown",
                    DepartmentID = user != null ? user.DepartmentID ?? "Unknown" : "Unknown"
                }
            ).ToListAsync();

            return leaves;
        }
    }
}
