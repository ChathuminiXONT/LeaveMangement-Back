using LMS.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Interfaces
{
    public interface IRecentActivityRepository
    {
        Task<IEnumerable<RecentActivityDto>> GetRecentActivitiesAsync(string email);
    }
}
