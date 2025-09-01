//using LMS.Domain.Models;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Reflection.Emit;

//namespace LMS.Infrastructure.Data
//{
//    public class LMSDbContext : DbContext
//    {
//        public LMSDbContext(DbContextOptions<LMSDbContext> options) : base(options) { }

//        public DbSet<User> Users { get; set; }
//        public DbSet<Departments> Departments { get; set; }
//        public DbSet<LeaveTypes> LeaveTypes { get; set; }
//        public DbSet<LeaveDetails> LeaveDetails { get; set; }
//        public DbSet<LeaveEntitle> LeaveEntitlements { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            // User
//            modelBuilder.Entity<User>(entity =>
//            {
//                entity.ToTable("XDUsers", "dbo");
//                entity.HasKey(e => e.LoginID);
//                entity.Property(e => e.LoginID).HasColumnName("LoginID");
//                entity.Property(e => e.UserName).HasColumnName("UserName");
//                entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");
//                entity.Property(e => e.IsActive).HasColumnName("IsActive");
//                entity.Property(e => e.AuthenticLevel).HasColumnName("AuthenticLevel");
//                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
//                entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
//                entity.Property(e => e.LastUpdatedBy).HasColumnName("LastUpdatedBy");
//                entity.Property(e => e.LastUpdatedOn).HasColumnName("LastUpdatedOn");
//                entity.Property(e => e.Password).HasColumnName("Password");
//                entity.Property(e => e.EmailAddress).HasColumnName("EmailAddress");
//                entity.Property(e => e.UserID).HasColumnName("UserID");
//                entity.Property(e => e.IsAvailable).HasColumnName("IsAvailable");
//                entity.Property(e => e.IsPDA).HasColumnName("IsPDA");
//                entity.Property(e => e.LeaveSystemAdmin).HasColumnName("LeaveSystemAdmin");

//                entity.HasOne(u => u.Department)
//                    .WithMany()
//                    .HasForeignKey(u => u.DepartmentID)
//                    .HasPrincipalKey(d => d.Department); // Department is the string code, not RecID
//            });

//            // Departments
//            modelBuilder.Entity<Departments>(entity =>
//            {
//                entity.ToTable("Department", "HR");
//                entity.HasKey(e => e.RecID);
//                entity.Property(e => e.RecID).HasColumnName("RecID");
//                entity.Property(e => e.BusinessUnit).HasColumnName("BusinessUnit");
//                entity.Property(e => e.Department).HasColumnName("Department");
//                entity.Property(e => e.DepName).HasColumnName("DepName");
//                entity.Property(e => e.ApproverEmp1).HasColumnName("ApproveEmp1");
//                entity.Property(e => e.ApproverEmp2).HasColumnName("ApproveEmp2");
//                entity.Property(e => e.Status).HasColumnName("Status");
//                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
//                entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
//                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
//                entity.Property(e => e.UpdatedOn).HasColumnName("UpdatedOn");
//            });

//            // LeaveDetails
//            modelBuilder.Entity<LeaveDetails>(entity =>
//            {
//                entity.ToTable("LeaveDetail", "HR");
//                entity.HasKey(e => e.RecID);
//                entity.Property(e => e.RecID).HasColumnName("RecID");
//                entity.Property(e => e.BusinessUnit).HasColumnName("BusinessUnit");
//                entity.Property(e => e.EmployeeNo).HasColumnName("EmployeeNo");
//                entity.Property(e => e.EmpEmailID).HasColumnName("EmpEmailID");
//                entity.Property(e => e.LeaveYear).HasColumnName("LeaveYear");
//                entity.Property(e => e.LeaveType).HasColumnName("LeaveType");
//                entity.Property(e => e.LeaveDays).HasColumnName("LeaveDays");
//                entity.Property(e => e.LeaveStart).HasColumnName("LeaveStart");
//                entity.Property(e => e.StartTime).HasColumnName("StartTime");
//                entity.Property(e => e.LeaveEnd).HasColumnName("LeaveEnd");
//                entity.Property(e => e.EndTime).HasColumnName("EndTime");
//                entity.Property(e => e.LeaveReason).HasColumnName("LeaveReason");
//                entity.Property(e => e.LeaveAppliedOn).HasColumnName("LeaveAppliedOn");
//                entity.Property(e => e.LeaveStatus).HasColumnName("LeaveStatus");
//                entity.Property(e => e.ApprovedComment).HasColumnName("ApprovedComment");
//                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
//                entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
//                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
//                entity.Property(e => e.UpdatedOn).HasColumnName("UpdatedOn");
//            });

//            // LeaveEntitle
//            modelBuilder.Entity<LeaveEntitle>(entity =>
//            {
//                entity.ToTable("LeaveEntitle", "HR");
//                entity.HasKey(e => e.RecID);
//                entity.Property(e => e.RecID).HasColumnName("RecID");
//                entity.Property(e => e.BusinessUnit).HasColumnName("BusinessUnit");
//                entity.Property(e => e.EmpNo).HasColumnName("EmpNo");
//                entity.Property(e => e.EmpEmailID).HasColumnName("EmpEmailID");
//                entity.Property(e => e.LVYear).HasColumnName("LVYear");
//                entity.Property(e => e.LeaveType).HasColumnName("LeaveType");
//                entity.Property(e => e.EntitledLeave).HasColumnName("EntitledLeave");
//                entity.Property(e => e.TakenLeaves).HasColumnName("TakenLeave");
//                entity.Property(e => e.AvailableLeave).HasColumnName("AvailableLeave");
//                entity.Property(e => e.RequestedLeave).HasColumnName("RequestedLeave");
//                entity.Property(e => e.RejectedLeave).HasColumnName("RejectedLeave");
//                entity.Property(e => e.SuspendedLeave).HasColumnName("SuspendedLeave");
//                entity.Property(e => e.Status).HasColumnName("Status");
//                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
//                entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
//                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
//                entity.Property(e => e.UpdatedOn).HasColumnName("UpdatedOn");
//            });

//            // LeaveTypes
//            modelBuilder.Entity<LeaveTypes>(entity =>
//            {
//                entity.ToTable("LeaveTypes", "HR");
//                entity.HasKey(e => e.RecID);
//                entity.Property(e => e.RecID).HasColumnName("RecID");
//                entity.Property(e => e.BusinessUnit).HasColumnName("BusinessUnit");
//                entity.Property(e => e.LeaveType).HasColumnName("LeaveType");
//                entity.Property(e => e.LeaveTypeName).HasColumnName("LeaveTypeName");
//                entity.Property(e => e.DfltLVDays).HasColumnName("DfltLVDays");
//                entity.Property(e => e.LeaveColor).HasColumnName("LeaveColor");
//                entity.Property(e => e.Status).HasColumnName("Status");
//                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
//                entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
//                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
//                entity.Property(e => e.UpdatedOn).HasColumnName("UpdatedOn");
//                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
//            });
//        }
//    }
//}
