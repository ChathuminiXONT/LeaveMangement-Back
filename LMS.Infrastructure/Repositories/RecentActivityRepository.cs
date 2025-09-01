using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                .Where(l => l.EmpEmailID == email && l.LeaveAppliedOn != null)
                .OrderByDescending(l => l.LeaveAppliedOn.Value)
                .Take(10)
                .Select(l => new RecentActivityDto
                {
                    LeaveReason = l.LeaveReason ?? string.Empty,
                    LeaveAppliedOn = l.LeaveAppliedOn ?? default,
                    LeaveStatus = l.LeaveStatus ?? string.Empty,
                    UpdatedBy = l.UpdatedBy ?? string.Empty
                })
                .ToListAsync();
        }
    }
}
