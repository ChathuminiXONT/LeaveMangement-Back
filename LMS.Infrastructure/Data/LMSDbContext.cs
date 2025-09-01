using System;
using Microsoft.EntityFrameworkCore;
using LMS.Domain.Models;

namespace LMS.Infrastructure.Data
{
    public class LMSDbContext : DbContext
    {
        public LMSDbContext(DbContextOptions<LMSDbContext> options) : base(options)
        {
        }

        // Tables
        public DbSet<User> XDUsers { get; set; }
        public DbSet<LeaveDetail> LeaveDetails { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<LeaveTypes> LeaveTypes { get; set; }
        public DbSet<LeaveEntitle> LeaveEntitlements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ?? User mapping (dbo.XDUsers)
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("XDUsers", "dbo"); // table name is dbo.XDUser

                entity.HasKey(e => e.EmailAddress); // PK is EmailAddress (string)

                entity.Property(e => e.EmailAddress).HasColumnName("EmailAddress");
                entity.Property(e => e.Password).HasColumnName("Password");
                entity.Property(e => e.UserName).HasColumnName("UserName");
                entity.Property(e => e.IsActive).HasColumnName("IsActive");
                entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");
            });

            // ?? LeaveDetail mapping (HR.LeaveDetail)
            modelBuilder.Entity<LeaveDetail>(entity =>
            {
                entity.ToTable("LeaveDetail", "HR");
                entity.HasKey(e => e.RecID); // Use RecID as PK

                entity.Property(e => e.RecID).HasColumnName("RecID");
                entity.Property(e => e.EmpEmailID).HasColumnName("EmpEmailID"); // normal column
                entity.Property(e => e.LeaveReason).HasColumnName("LeaveReason");
                entity.Property(e => e.LeaveAppliedOn).HasColumnName("LeaveAppliedOn");
                entity.Property(e => e.LeaveStatus).HasColumnName("LeaveStatus");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
                entity.Property(e => e.LeaveStart).HasColumnName("LeaveStart");
                entity.Property(e => e.StartTime).HasColumnName("StartTime");
                entity.Property(e => e.LeaveEnd).HasColumnName("LeaveEnd");
                entity.Property(e => e.EndTime).HasColumnName("EndTime");
                entity.Property(e => e.LeaveType).HasColumnName("LeaveType");
            });

            // LeaveEntitle
            modelBuilder.Entity<LeaveEntitle>(entity =>
            {
                entity.ToTable("LeaveEntitle", "HR");
                entity.HasKey(e => e.RecID);
                entity.Property(e => e.RecID).HasColumnName("RecID");
                entity.Property(e => e.BusinessUnit).HasColumnName("BusinessUnit");
                entity.Property(e => e.EmpNo).HasColumnName("EmpNo");
                entity.Property(e => e.EmpEmailID).HasColumnName("EmpEmailID");
                entity.Property(e => e.LVYear).HasColumnName("LVYear");
                entity.Property(e => e.LeaveType).HasColumnName("LeaveType");
                entity.Property(e => e.EntitledLeave).HasColumnName("EntitledLeave");
                entity.Property(e => e.TakenLeaves).HasColumnName("TakenLeave");
                entity.Property(e => e.AvailableLeave).HasColumnName("AvailableLeave");
                entity.Property(e => e.RequestedLeave).HasColumnName("RequestedLeave");
                entity.Property(e => e.RejectedLeave).HasColumnName("RejectedLeave");
                entity.Property(e => e.SuspendedLeave).HasColumnName("SuspendedLeave");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
                entity.Property(e => e.UpdatedOn).HasColumnName("UpdatedOn");
            });

            // LeaveTypes
            modelBuilder.Entity<LeaveTypes>(entity =>
            {
                entity.ToTable("LeaveTypes", "HR");
                entity.HasKey(e => e.RecID);
                entity.Property(e => e.RecID).HasColumnName("RecID");
                entity.Property(e => e.BusinessUnit).HasColumnName("BusinessUnit");
                entity.Property(e => e.LeaveType).HasColumnName("LeaveType");
                entity.Property(e => e.LeaveTypeName).HasColumnName("LeaveTypeName");
                entity.Property(e => e.DfltLVDays).HasColumnName("DfltLVDays");
                entity.Property(e => e.LeaveColor).HasColumnName("LeaveColor");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
                entity.Property(e => e.UpdatedOn).HasColumnName("UpdatedOn");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
            });
            // Departments
            modelBuilder.Entity<Departments>(entity =>
            {
                entity.ToTable("Department", "HR");
                entity.HasKey(e => e.RecID);
                entity.Property(e => e.RecID).HasColumnName("RecID");
                entity.Property(e => e.BusinessUnit).HasColumnName("BusinessUnit");
                entity.Property(e => e.Department).HasColumnName("Department");
                entity.Property(e => e.DepName).HasColumnName("DepName");
                entity.Property(e => e.ApproverEmp1).HasColumnName("ApproveEmp1");
                entity.Property(e => e.ApproverEmp2).HasColumnName("ApproveEmp2");
                entity.Property(e => e.Status).HasColumnName("Status");
                entity.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(e => e.CreatedOn).HasColumnName("CreatedOn");
                entity.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
                entity.Property(e => e.UpdatedOn).HasColumnName("UpdatedOn");
            });
        }
    }
}