using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Models;

namespace LMS.Infrastructure.Repositories
{
    public class RecentActivityRepository : IRecentActivityRepository
    {
        private readonly LMSDbContext _context;

        public RecentActivityRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RecentActivityDto>> GetRecentActivitiesAsync(string email)
        {
            return await _context.LeaveDetails
                .Where(l => l.EmpEmailID == email)   // 🔹 filter by email
                .OrderByDescending(l => l.LeaveAppliedOn)
                .Take(10)
                .Select(l => new RecentActivityDto
                {
                    LeaveReason = l.LeaveReason,
                    LeaveAppliedOn = l.LeaveAppliedOn,
                    LeaveStatus = l.LeaveStatus,
                    UpdatedBy = l.UpdatedBy
                })
                .ToListAsync();
        }
    }
}
