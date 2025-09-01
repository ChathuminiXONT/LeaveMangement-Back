using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models
{

    [Table("LeaveEntitle", Schema = "HR")]
    public class LeaveEntitle
    {
        [Key]
        public long RecID { get; set; }

        [StringLength(4)]
        [Required]
        public string BusinessUnit { get; set; } = "";

        [StringLength(20)]
        [Required]
        public string EmpNo { get; set; } = "";

        [StringLength(50)]
        [Required]
        public string EmpEmailID { get; set; } = "";

        [Required]
        public int LVYear { get; set; } = 0;

        [StringLength(1)]
        [Required]
        public string LeaveType { get; set; } = "";

        [Required]
        public int EntitledLeave { get; set; } = 0;

        [Column("TakenLeave")]
        [Required]
        public int TakenLeaves { get; set; } = 0;

        [Required]
        public int AvailableLeave { get; set; } = 0;

        [Required]
        public int RequestedLeave { get; set; } = 0;

        [Required]
        public int RejectedLeave { get; set; } = 0;

        [Required]
        public int SuspendedLeave { get; set; } = 0;

        [StringLength(1)]
        [Required]
        public string Status { get; set; } = "0";

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
