using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class LeaveApplicationResponseDto
    {
        public long RecID { get; set; }
        public string BusinessUnit { get; set; } = "";
        public string EmployeeNo { get; set; } = "";
        public string EmpEmailID { get; set; } = "";
        public int LeaveYear { get; set; }
        public string LeaveType { get; set; } = "";
        public string LeaveTypeName { get; set; } = "";
        public int LeaveDays { get; set; }
        public DateTime LeaveStart { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime LeaveEnd { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string LeaveReason { get; set; } = "";
        public DateTime LeaveAppliedOn { get; set; }
        public string LeaveStatus { get; set; } = "";
        public string LeaveStatusText { get; set; } = "";
        public string ApprovedComment { get; set; } = "";
        public string ApproverName { get; set; } = "";
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
