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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🔹 User mapping (dbo.XDUsers)
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

            // 🔹 LeaveDetail mapping (HR.LeaveDetail)
            modelBuilder.Entity<LeaveDetail>(entity =>
            {
                entity.ToTable("LeaveDetail", "HR"); 

                entity.HasKey(e => e.EmpEmailID); // assuming primary key column is Id

                
                entity.Property(e => e.EmpEmailID).HasColumnName("EmpEmailID");
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
        }
    }
}
