using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class PendingLeaveRequestDto
    {
        //public int RecID { get; set; }
        //public string EmployeeName { get; set; }
        //public string EmpNo { get; set; }
        //public string LeaveType { get; set; }
        //public string LeaveTypeName { get; set; }
        //public int LeaveDays { get; set; }
        //public int AvailableLeaves { get; set; }
        //public DateTime LeaveStart { get; set; }
        //public DateTime LeaveEnd { get; set; }
        //public DateTime StartTime { get; set; }
        //public DateTime EndTime { get; set; }
        //public string LeaveReason { get; set; }
        //public int Status { get; set; }
        public long RecID { get; set; }
        public string employee { get; set; }
        public string EmpEmailID { get; set; }
        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public TimeSpan? startTime { get; set; }
        public TimeSpan? endTime { get; set; }
        public int days { get; set; }
        public string reason { get; set; }
        public string leaveType { get; set; }
        public string status { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int leaveBalance { get; set; }
    }
}
