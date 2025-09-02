using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models
{
    public class User
    {
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? UserName { get; set; }
        public string? IsActive { get; set; }
        public string? DepartmentID { get; set; } // backend uses PascalCase
        public int? UserID { get; set; }
        public string? LoginID { get; set; }
    }
}
