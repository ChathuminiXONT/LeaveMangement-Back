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
                where l.LeaveStart.Year == year && l.LeaveStart.Month == month
                select new LeaveDto
                {
                    LeaveStart = l.LeaveStart,
                    StartTime = l.StartTime.ToString(@"hh\:mm"),
                    LeaveEnd = l.LeaveEnd,
                    EndTime = l.EndTime.ToString(@"hh\:mm"),
                    LeaveType = l.LeaveType,
                    UserName = user != null ? user.UserName : "Unknown",
                    DepartmentID = user != null ? user.DepartmentID : "Unknown"
                }
            ).ToListAsync();

            return leaves;
        }
    }
}
