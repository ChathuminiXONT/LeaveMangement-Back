using LMS.Domain.DTOs;
using LMS.Domain.Models;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly LMSDbContext _context;

        public UsersRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return await _context.XDUsers
                .FirstOrDefaultAsync(u => u.EmailAddress.ToLower() == email.ToLower());
        }

        public async Task<UserApproverDetailsDTO> GetUserApproverDetailsAsync(string email)
        {
            var departments = await _context.Departments
                .Where(d =>
                    d.ApproverEmp1.ToLower() == email.ToLower() ||
                    d.ApproverEmp2.ToLower() == email.ToLower())
                .ToListAsync();

            var isApprover = departments.Any();

            
            var departmentDTOs = departments.Select(d => new LMS.Domain.DTOs.Departments
            {
                DepartmentId = d.Department
            
            }).ToList();
            string approverLevel = departments.Any(d => d.ApproverEmp1.ToLower() == email.ToLower()) ? "Approver1" : "Approver2";
            return new UserApproverDetailsDTO
            {
                IsApprover = isApprover,
                ApproverEmail = email,
                ApproverLevel = approverLevel,
                Departments = departmentDTOs
            };
        }
    }
}

