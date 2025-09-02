using LMS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Infrastructure.Data;
using LMS.Domain.Models;
using LMS.Infrastructure.Repositories;

namespace LMS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _userRepository.ValidateUser(email, password);

            return user;
        }
    }
}

// LMS.Domain/Interfaces/IUserService.cs
namespace LMS.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
    }
}
