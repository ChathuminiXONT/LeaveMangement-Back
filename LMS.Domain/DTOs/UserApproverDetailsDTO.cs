using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class UserApproverDetailsDTO
    {
        public bool IsApprover { get; set; }
        public string? ApproverEmail { get; set; }
        public string? ApproverLevel { get; set; }
        public List<Departments> Departments { get; set; } = new List<Departments>();
    }

    public class Departments
    {
        public string DepartmentId { get; set; }

    }
}