using LMS.Domain.DTOs;
using LMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<UserApproverDetailsDTO> GetUserApproverDetailsAsync(string email);


    }
}
