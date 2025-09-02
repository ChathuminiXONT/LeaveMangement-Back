using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LMS.Application.Services
{
    public class RecentActivityService
    {
        private readonly IRecentActivityRepository _recentActivityRepository;

        public RecentActivityService(IRecentActivityRepository recentActivityRepository)
        {
            _recentActivityRepository = recentActivityRepository;
        }

        public async Task<IEnumerable<RecentActivityDto>> GetRecentActivitiesAsync(string email)
        {
            return await _recentActivityRepository.GetRecentActivitiesAsync(email);
        }
    }

}
