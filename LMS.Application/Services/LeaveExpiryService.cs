using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Services
{
    public class LeaveExpiryService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public LeaveExpiryService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateExpiredLeaves();
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run daily
            }
        }

        private async Task UpdateExpiredLeaves()
        {
            try
            {
                using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                using var command = new SqlCommand("UpdateExpiredLeaveRequests", connection)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();

                Console.WriteLine($"Updated {result} expired leave requests");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
