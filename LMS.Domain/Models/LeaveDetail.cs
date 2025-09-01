using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Domain.Models
{
    [Table("LeaveDetail", Schema = "HR")]
    public class LeaveDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RecID { get; set; } 

        [Column("EmpEmailID")]
        public string? EmpEmailID { get; set; } 

        [Column("LeaveStart")]
        public DateTime? LeaveStart { get; set; } 
        [Column("StartTime")]
        public TimeSpan? StartTime { get; set; } 

        [Column("LeaveEnd")]
        public DateTime? LeaveEnd { get; set; } 

        [Column("EndTime")]
        public TimeSpan? EndTime { get; set; } 

        [Column("LeaveType")]
        public string? LeaveType { get; set; } 

        [Column("LeaveReason")]
        public string? LeaveReason { get; set; } 

        [Column("LeaveAppliedOn")]
        public DateTime? LeaveAppliedOn { get; set; } 

        [Column("LeaveStatus")]
        public string? LeaveStatus { get; set; } 

        [Column("UpdatedBy")]
        public string? UpdatedBy { get; set; } 

        [StringLength(4)]
        [Required]
        public string BusinessUnit { get; set; } = "";

        [StringLength(20)]
        [Required]
        public string EmployeeNo { get; set; } = "";

        [Required]
        public int LeaveYear { get; set; } = 0;

        [Required]
        public int LeaveDays { get; set; } = 0;

        [StringLength(200)]
        [Required]
        public string ApprovedComment { get; set; } = "";

        [StringLength(40)]
        [Required]
        public string CreatedBy { get; set; } = "";

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.Now;


        public DateTime? UpdatedOn { get; set; }

    }
}
