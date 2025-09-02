using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs
{
    public class LeaveApplicationDto
    {
        public long? RecID { get; set; }

        
        //public string BusinessUnit { get; set; } = "";
        public string BusinessUnit { get; set; } = "XONT";

     
        public string EmployeeNo { get; set; } = "";

        [Required]
        public string EmpEmailID { get; set; } = "";

        [Required]
        public int LeaveYear { get; set; }

        [Required]
        public string LeaveType { get; set; } = "";

        [Required]
        public int LeaveDays { get; set; }

        [Required]
        public DateTime LeaveStart { get; set; }

        // Simple string for time - no extra files needed
        public string? StartTime { get; set; }  // Format: "09:00" or "14:30:00"

        [Required]
        public DateTime LeaveEnd { get; set; }

        // Simple string for time - no extra files needed
        public string? EndTime { get; set; }    // Format: "17:00" or "18:30:00"

        [Required]
        [StringLength(200)]
        public string LeaveReason { get; set; } = "";

        public string? ApproverEmail { get; set; }

        public bool IsHalfDay { get; set; } = false;
    }
}