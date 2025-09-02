using LMS.Domain.DTOs;
using LMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Repositories
{
    public interface IUsersRepository
    {
      
        Task<User> GetUserByEmailAsync(string email);

        Task<UserApproverDetailsDTO> GetUserApproverDetailsAsync(string email); 
    }
}
