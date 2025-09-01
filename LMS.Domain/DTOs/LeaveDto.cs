using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class LeaveDto
    {
        public DateTime LeaveStart { get; set; }
        public string StartTime { get; set; } // string for Angular
        public DateTime LeaveEnd { get; set; }
        public string EndTime { get; set; }   // string for Angular
        public string LeaveType { get; set; }
        public string UserName { get; set; }
        public string DepartmentID { get; set; }
    }
}
