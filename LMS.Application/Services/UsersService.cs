using LMS.Domain.DTOs;
using LMS.Domain.Models;
using LMS.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Departments = LMS.Domain.DTOs.Departments;

namespace LMS.Application.Services
{
    public class UsersService:IUsersService
    {
        private readonly IUsersRepository _userRepository;
        public UsersService(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task<User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;
            var user = await _userRepository.GetUserByEmailAsync(email);
            return user;
        }



        public async Task<UserApproverDetailsDTO> GetUserApproverDetailsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new UserApproverDetailsDTO { IsApprover = false, ApproverEmail = email, Departments = new List<Departments>() };

            return await _userRepository.GetUserApproverDetailsAsync(email);
        }
    }
}
