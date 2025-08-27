using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models
{
    [Table("LeaveDetail", Schema = "HR")]
    public class LeaveDetails
    {
        [Key]
        public long RecID { get; set; }

        [StringLength(4)]
        [Required]
        public string BusinessUnit { get; set; } = "";

        [StringLength(20)]
        [Required]
        public string EmployeeNo { get; set; } = "";

        [StringLength(50)]
        [Required]
        public string EmpEmailID { get; set; } = "";

        [Required]
        public int LeaveYear { get; set; } = 0;

        [StringLength(1)]
        [Required]
        public string LeaveType { get; set; } = "";

        [Required]
        public int LeaveDays { get; set; } = 0;

        [Column(TypeName = "date")]
        [Required]
        public DateTime LeaveStart { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? StartTime { get; set; }

        [Column(TypeName = "date")]
        [Required]
        public DateTime LeaveEnd { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? EndTime { get; set; }

        [StringLength(200)]
        [Required]
        public string LeaveReason { get; set; } = "";

        [Column(TypeName = "date")]
        [Required]
        public DateTime LeaveAppliedOn { get; set; }

        [StringLength(1)]
        [Required]
        public string LeaveStatus { get; set; } = "0"; // 0=Pending, 1=Approved by Emp1, 2=Approved by Emp2, 3=Rejected by Emp1, 4=Rejected by Emp2

        [StringLength(200)]
        [Required]
        public string ApprovedComment { get; set; } = "";

        [StringLength(40)]
        [Required]
        public string CreatedBy { get; set; } = "";

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [StringLength(40)]
        [Required]
        public string UpdatedBy { get; set; } = "";

        public DateTime? UpdatedOn { get; set; }
    }
}
