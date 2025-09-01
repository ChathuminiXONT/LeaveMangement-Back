using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class RecentActivityDto
    {
        public string LeaveReason { get; set; }
        public DateTime LeaveAppliedOn { get; set; }
        public string LeaveStatus { get; set; }
        public string UpdatedBy { get; set; }
    }
}
