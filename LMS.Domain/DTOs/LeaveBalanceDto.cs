using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class LeaveBalanceDto
    {
        public string LeaveType { get; set; } = "";
        public string LeaveTypeName { get; set; } = "";
        public int EntitledLeave { get; set; }
        public int TakenLeaves { get; set; }
        public int AvailableLeave { get; set; }
        public int RequestedLeave { get; set; }
        public int RejectedLeave { get; set; }
        public string LeaveColor { get; set; } = "";
    }
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public T? Data { get; set; }
    }
}
