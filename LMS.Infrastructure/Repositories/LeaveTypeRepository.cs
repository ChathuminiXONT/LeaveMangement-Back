using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using LMS.Domain.Models;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Repositories
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly LMSDbContext _context;
        private readonly ILogger<LeaveTypeRepository> _logger;

        public LeaveTypeRepository(LMSDbContext context, ILogger<LeaveTypeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<LeaveTypes>> GetActiveLeaveTypesAsync()
        {
            try
            {
                return await _context.LeaveTypes
                    .Where(lt => lt.Status == "1")
                    .OrderBy(lt => lt.LeaveTypeName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active leave types");
                return new List<LeaveTypes>();
            }
        }

        public async Task<LeaveTypes?> GetLeaveTypeByIdAsync(string leaveType)
        {
            try
            {
                return await _context.LeaveTypes
                    .FirstOrDefaultAsync(lt => lt.LeaveType == leaveType && lt.Status == "1");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave type {LeaveType}", leaveType);
                return null;
            }
        }
    }
}
