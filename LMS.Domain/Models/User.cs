//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LMS.Domain.Models
//{
//    [Table("XDUsers", Schema = "dbo")]
//    public class User
//    {
//        [Key]
//        [StringLength(15)]
//        public string? LoginID { get; set; }

//        [StringLength(30)]
//        public string? UserName { get; set; }

//        [StringLength(3)]
//        public string? DepartmentID { get; set; }

//        [StringLength(1)]
//        public string? IsActive { get; set; }

//        [StringLength(1)]
//        public string? AuthenticLevel { get; set; }

//        [StringLength(15)]
//        public string? CreatedBy { get; set; }

//        public DateTime? CreatedOn { get; set; }

//        [StringLength(15)]
//        public string? LastUpdatedBy { get; set; }

//        public DateTime? LastUpdatedOn { get; set; }

//        [StringLength(30)]
//        public string? Password { get; set; }

//        [StringLength(30)]
//        public string? EmailAddress { get; set; }

//        public int UserID { get; set; }

//        [StringLength(1)]
//        public string? IsAvailable { get; set; }

//        [StringLength(1)]
//        public string? IsPDA { get; set; }

//        [StringLength(1)]
//        public string LeaveSystemAdmin { get; set; }

//        // Navigation property
//        [ForeignKey("DepartmentID")]
//        public virtual Departments Department { get; set; }
//    }
//}
using System;

namespace LMS.Domain.Models
{
    public class User
    {
        public string? EmailAddress { get; set; }  // Nullable
        public string? Password { get; set; }      // Nullable
        public string? UserName { get; set; }      // Nullable
        public string? IsActive { get; set; }      // Nullable
        public string? DepartmentID { get; set; }  // Nullable
        public int? UserID { get; set; }
        public string? LoginID { get; set; }
    }
}