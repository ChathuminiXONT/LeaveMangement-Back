using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class PendingRequestsCountDto
    {
        public int PendingCount { get; set; }
        public string DepartmentName { get; set; }
        public string ApproverName { get; set; }
    }
}
