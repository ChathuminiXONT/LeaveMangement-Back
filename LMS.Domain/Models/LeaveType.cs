using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("LeaveTypes", Schema = "HR")]
public class LeaveTypes
{
    [Key]
    public long RecID { get; set; }

    [StringLength(4)]
    [Required]
    public string BusinessUnit { get; set; } = "";

    [StringLength(1)]
    [Required]
    public string LeaveType { get; set; } = "";

    [StringLength(50)]
    [Required]
    public string LeaveTypeName { get; set; } = "";

    [Column("DfltLVDays")]
    [Required]
    public int DfltLVDays { get; set; } = 0;

    [StringLength(50)]
    [Required]
    public string LeaveColor { get; set; } = "";

    [StringLength(1)]
    [Required]
    public string Status { get; set; } = "0";

    [StringLength(50)]
    [Required]
    public string CreatedBy { get; set; } = "";

    [Required]
    public DateTime CreatedOn { get; set; } = DateTime.Now;

    [StringLength(50)]
    [Required]
    public string UpdatedBy { get; set; } = "";

    public DateTime? UpdatedOn { get; set; }

    [Timestamp]
    public byte[] Timestamp { get; set; }
}