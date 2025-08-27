using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Domain.Models
{
    [Table("LeaveDetail", Schema = "HR")]
    public class LeaveDetail
    {
        [Key] // You may change to LeaveID if you have it
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("EmpEmailID")]
        public string EmpEmailID { get; set; }

        [Column("LeaveStart")]
        public DateTime LeaveStart { get; set; }

        [Column("StartTime")]
        public TimeSpan StartTime { get; set; }

        [Column("LeaveEnd")]
        public DateTime LeaveEnd { get; set; }

        [Column("EndTime")]
        public TimeSpan EndTime { get; set; }

        [Column("LeaveType")]
        public string LeaveType { get; set; }

        [Column("LeaveReason")] public string LeaveReason { get; set; }
        [Column("LeaveAppliedOn")] public DateTime LeaveAppliedOn { get; set; }
        [Column("LeaveStatus")] public string LeaveStatus { get; set; }
        [Column("UpdatedBy")] public string UpdatedBy { get; set; }
    }
}
