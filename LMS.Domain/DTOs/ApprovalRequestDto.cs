using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class ApprovalRequestDto
    {
        public long RecId { get; set; }
        public string EmployeeEmail { get; set; }
        public string ApproverEmail { get; set; }
        public string ApproverLevel { get; set; } // 1 for Approver1, 2 for Approver2
        public int LeaveDays { get; set; }
        public string ApproverComment { get; set; }
    }
}
