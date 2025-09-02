using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using LMS.Domain.Interfaces;
using LMS.Domain.Models;

namespace LMS.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LMSDbContext _context;

        public UserRepository(LMSDbContext context)
        {
            _context = context;
        }

        public async Task<User> ValidateUser(string email, string password)
        {
            var user = await _context.XDUsers
                .Where(u => u.EmailAddress == email
                         && u.Password == password
                         && u.IsActive == "1")
                .Select(u => new User
                {
                    EmailAddress = u.EmailAddress,
                    UserName = u.UserName,
                    IsActive = u.IsActive,
                    DepartmentID = u.DepartmentID
                })
                .FirstOrDefaultAsync();

            if (user != null)
            {
                // ✅ Check if user is Department Approver (Admin)
                bool isAdmin = await _context.Departments
                    .AnyAsync(d => d.ApproverEmp1 == email || d.ApproverEmp2 == email);

                user.IsAdmin = isAdmin;
            }

            return user;
        }
    }
}
