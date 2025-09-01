using LMS.Domain.Models;
using System.Threading.Tasks;

namespace LMS.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> ValidateUser(string email, string password);
    }
}
