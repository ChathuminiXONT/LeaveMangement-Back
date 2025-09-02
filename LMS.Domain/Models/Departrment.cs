//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace LMS.Domain.Models
//{
//    [Table("Department", Schema = "HR")]
//    public class Departments
//    {
//        [Key]
//        public long RecID { get; set; }

//        [StringLength(4)]
//        [Required]
//        public string BusinessUnit { get; set; } = "";

//        [StringLength(10)]
//        [Required]
//        public string Department { get; set; } = "";

//        [Column("DepName")]
//        [StringLength(100)]
//        [Required]
//        public string DepName { get; set; } = "";

//        [Column("ApproveEmp1")]
//        [StringLength(50)]
//        [Required]
//        public string ApproverEmp1 { get; set; } = "";

//        [Column("ApproveEmp2")]
//        [StringLength(50)]
//        [Required]
//        public string ApproverEmp2 { get; set; } = "";

//        [StringLength(1)]
//        [Required]
//        public string Status { get; set; } = "0";

//        [StringLength(40)]
//        [Required]
//        public string CreatedBy { get; set; } = "";

//        [Required]
//        public DateTime CreatedOn { get; set; } = DateTime.Now;

//        [StringLength(40)]
//        [Required]
//        public string UpdatedBy { get; set; } = "";

//        public DateTime? UpdatedOn { get; set; }

//        // ✅ Add this navigation property
//        public virtual ICollection<User> Users { get; set; } = new List<User>();
//    }
//}
