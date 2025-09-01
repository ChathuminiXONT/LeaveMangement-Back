using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class LeaveApprovalDto
    {
        [Required]
        public long RecID { get; set; }

        [Required]
        public string LeaveStatus { get; set; } = ""; // 1=Approved, 3=Rejected

        [StringLength(200)]
        public string ApprovedComment { get; set; } = "";

        [Required]
        public string ApproverEmail { get; set; } = "";
    }
}
