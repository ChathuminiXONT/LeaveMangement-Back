using System;

namespace LMS.Domain.Models
{
    public class User
    {
        public string? EmailAddress { get; set; }  // Nullable
        public string? Password { get; set; }      // Nullable
        public string? UserName { get; set; }      // Nullable
        public string? IsActive { get; set; }      // Nullable
        public string? DepartmentID { get; set; }  // Nullable
    }
}
