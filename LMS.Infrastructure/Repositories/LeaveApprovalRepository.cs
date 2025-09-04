using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using LMS.Domain.Models;
using LMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IEmailService = LMS.Domain.Interfaces.IEmailService;


namespace LMS.Infrastructure.Repositories
{
    public class LeaveApprovalRepository: ILeaveApprovalRepository
    {
        private readonly LMSDbContext lMSDbContext;
        private readonly IEmailService _emailService;


        public LeaveApprovalRepository(LMSDbContext lMSDbContext, IEmailService emailService)
        {
            this.lMSDbContext = lMSDbContext;
            _emailService = emailService;
        }
        public async Task<List<PendingLeaveRequestDto>> GetLeaveDetailsByDepartmentAsync(string departmentId)
        {
            var today = DateTime.Today;
            Console.WriteLine($"Today is: {today}");
            return await lMSDbContext.LeaveDetails
                .Join(
                    lMSDbContext.XDUsers,
                    ld => ld.EmpEmailID.Trim(),
                    u => u.EmailAddress != null ? u.EmailAddress.Trim() : "",
                    (ld, u) => new { LeaveDetail = ld, User = u }
                )
                .Join(
                    lMSDbContext.LeaveEntitlements,
                    x => new { Email = x.LeaveDetail.EmpEmailID.Trim(), LeaveType = x.LeaveDetail.LeaveType },
                    le => new { Email = le.EmpEmailID != null ? le.EmpEmailID.Trim() : "", LeaveType = le.LeaveType },
                    (x, le) => new { x.LeaveDetail, x.User, LeaveEntitle = le }
                )
                .Where(x =>
                   // x.LeaveDetail.LeaveStatus == "0" &&
                    x.User.DepartmentID == departmentId &&
                    !string.IsNullOrEmpty(x.LeaveDetail.EmpEmailID) &&
                    !string.IsNullOrEmpty(x.User.EmailAddress) &&
                      (
                // Pending requests
                x.LeaveDetail.LeaveStatus == "0" ||
                // Approved/Rejected requests processed today
                (
                    (x.LeaveDetail.LeaveStatus == "1" || x.LeaveDetail.LeaveStatus == "2" || x.LeaveDetail.LeaveStatus == "3" || x.LeaveDetail.LeaveStatus == "4") &&
                     x.LeaveDetail.UpdatedOn.HasValue &&
                    x.LeaveDetail.UpdatedOn.Value.Date == today
                )
            )
                )
                .Select(x => new PendingLeaveRequestDto
                {
                    RecID=x.LeaveDetail.RecID,
                    employee = x.User.UserName,
                    EmpEmailID = x.LeaveDetail.EmpEmailID,
                    from = x.LeaveDetail.LeaveStart ?? DateTime.MinValue,
                    to = x.LeaveDetail.LeaveEnd ?? DateTime.MinValue,
                    startTime= x.LeaveDetail.StartTime,
                    endTime= x.LeaveDetail.EndTime,
                    days = x.LeaveDetail.LeaveDays,
                    reason = x.LeaveDetail.LeaveReason,
                    leaveType = x.LeaveDetail.LeaveType,
                    status = x.LeaveDetail.LeaveStatus,
                    leaveBalance = x.LeaveEntitle.AvailableLeave
                })
                .ToListAsync();
        }
        public async Task<ApprovalResponseDto> ApproveLeaveRequestAsync(ApprovalRequestDto request)
        {
            using var transaction = await lMSDbContext.Database.BeginTransactionAsync();

            try
            {
                // Find the leave detail record
                var leaveDetail = await lMSDbContext.LeaveDetails
                    .FirstOrDefaultAsync(ld => ld.RecID == request.RecId);

                if (leaveDetail == null)
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Leave request not found.",
                        UpdatedStatus = 0
                    };
                }

               // Get employee details for email
                var employee = await lMSDbContext.XDUsers
                    .FirstOrDefaultAsync(u => u.EmailAddress.Trim() == request.EmployeeEmail.Trim());

                // Check if leave is still pending
                if (leaveDetail.LeaveStatus != "0")
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Leave request is not in pending status.",
                        UpdatedStatus = int.Parse(leaveDetail.LeaveStatus)
                    };
                }

                // Verify employee email matches
                if (leaveDetail.EmpEmailID?.Trim() != request.EmployeeEmail?.Trim())
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Employee email mismatch.",
                        UpdatedStatus = 0
                    };
                }
                // Find the approver user record to get the username
                var approverUser = await lMSDbContext.XDUsers
                    .FirstOrDefaultAsync(u => u.EmailAddress.Trim() == request.ApproverEmail.Trim());

                if (approverUser == null)
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Approver user not found.",
                        UpdatedStatus = 0
                    };
                }
                // Find the leave entitlement record
                var leaveEntitlement = await lMSDbContext.LeaveEntitlements
                         .FirstOrDefaultAsync(le =>
                         le.EmpEmailID.Trim() == request.EmployeeEmail.Trim() &&
                         le.LeaveType == leaveDetail.LeaveType &&
                         le.LVYear == leaveDetail.LeaveYear);

                if (leaveEntitlement == null)
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Leave entitlement record not found for the employee.",
                        UpdatedStatus = 0
                    };
                }

                // Check if employee has sufficient leave balance
                //if (leaveEntitlement.AvailableLeave < request.LeaveDays)
                //{
                //    return new ApprovalResponseDto
                //    {
                //        Success = false,
                //        Message = $"Insufficient leave balance. Available: {leaveEntitlement.AvailableLeave}, Requested: {request.LeaveDays}",
                //        UpdatedStatus = 0
                //    };
                //}

                // Update leave status based on approver level
                string newStatus = request.ApproverLevel == "Approver1" ? "1" : "2";
                leaveDetail.LeaveStatus = newStatus;

                // Add approver comment if provided
                if (!string.IsNullOrWhiteSpace(request.ApproverComment))
                {
                    leaveDetail.ApprovedComment = request.ApproverComment;
                }

                // Update timestamps and approver info
                leaveDetail.UpdatedOn = DateTime.Now;
                leaveDetail.UpdatedBy = approverUser.UserName;

                // Reduce available leave balance
                leaveEntitlement.AvailableLeave -= request.LeaveDays;
                leaveEntitlement.TakenLeaves += request.LeaveDays;
                leaveEntitlement.RequestedLeave -= request.LeaveDays;
                leaveEntitlement.UpdatedOn = DateTime.Now;

                // Save changes
                lMSDbContext.LeaveDetails.Update(leaveDetail);
                lMSDbContext.LeaveEntitlements.Update(leaveEntitlement);

                await lMSDbContext.SaveChangesAsync();
                // Send approval email
                try
                {
                    await _emailService.SendLeaveApprovalEmailAsync(
                        request.EmployeeEmail,
                        employee?.UserName ?? "Employee",
                        leaveDetail.LeaveStart ?? DateTime.MinValue,
                        leaveDetail.LeaveEnd ?? DateTime.MinValue,
                        leaveDetail.LeaveType,
                        request.ApproverEmail,
                        approverUser.UserName,
                        request.ApproverComment
                    );
                }
                catch (Exception emailEx)
                {
                    // Log email failure but don't fail the transaction
                    Console.WriteLine($"Email sending failed: {emailEx.Message}");
                }
                await transaction.CommitAsync();

                return new ApprovalResponseDto
                {
                    Success = true,
                    Message = $"Leave request approved successfully by approver level {request.ApproverLevel}.",
                    UpdatedStatus = int.Parse(newStatus)
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException($"Error approving leave request: {ex.Message}", ex);
            }
        }
        public async Task<ApprovalResponseDto> RejectLeaveRequestAsync(ApprovalRequestDto request)
        {
            using var transaction = await lMSDbContext.Database.BeginTransactionAsync();

            try
            {
                // Find the leave detail record
                var leaveDetail = await lMSDbContext.LeaveDetails
                    .FirstOrDefaultAsync(ld => ld.RecID == request.RecId);

                if (leaveDetail == null)
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Leave request not found.",
                        UpdatedStatus = 0
                    };
                }
                // Get employee details for email
                var employee = await lMSDbContext.XDUsers
                    .FirstOrDefaultAsync(u => u.EmailAddress.Trim() == request.EmployeeEmail.Trim());


                // Check if leave is still pending
                if (leaveDetail.LeaveStatus != "0")
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Leave request is not in pending status.",
                        UpdatedStatus = int.Parse(leaveDetail.LeaveStatus)
                    };
                }

                // Verify employee email matches
                if (leaveDetail.EmpEmailID?.Trim() != request.EmployeeEmail?.Trim())
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Employee email mismatch.",
                        UpdatedStatus = 0
                    };
                }
                // Find the approver user record to get the username
                var approverUser = await lMSDbContext.XDUsers
                    .FirstOrDefaultAsync(u => u.EmailAddress.Trim() == request.ApproverEmail.Trim());

                if (approverUser == null)
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Approver user not found.",
                        UpdatedStatus = 0
                    };
                }

                // Find the leave entitlement record
                var leaveEntitlement = await lMSDbContext.LeaveEntitlements
                         .FirstOrDefaultAsync(le =>
                         le.EmpEmailID.Trim() == request.EmployeeEmail.Trim() &&
                         le.LeaveType == leaveDetail.LeaveType &&
                         le.LVYear == leaveDetail.LeaveYear);

                if (leaveEntitlement == null)
                {
                    return new ApprovalResponseDto
                    {
                        Success = false,
                        Message = "Leave entitlement record not found for the employee.",
                        UpdatedStatus = 0
                    };
                }

                // Update leave status based on approver level (3 for Approver1, 4 for Approver2)
                string newStatus = request.ApproverLevel == "Approver1" ? "3" : "4";
                leaveDetail.LeaveStatus = newStatus;

                // Add approver comment if provided
                if (!string.IsNullOrWhiteSpace(request.ApproverComment))
                {
                    leaveDetail.ApprovedComment = request.ApproverComment;
                }

                // Update timestamps
                leaveDetail.UpdatedOn = DateTime.Now;
                leaveDetail.UpdatedBy = approverUser.UserName;

                // Update rejected leave column in leave entitlement table
                leaveEntitlement.RejectedLeave += request.LeaveDays;
                leaveEntitlement.UpdatedOn = DateTime.Now;

                // Save changes
                lMSDbContext.LeaveDetails.Update(leaveDetail);
                lMSDbContext.LeaveEntitlements.Update(leaveEntitlement);

                await lMSDbContext.SaveChangesAsync();
                // Send approval email
                try
                {
                    await _emailService.SendLeaveRejectionEmailAsync(
                        request.EmployeeEmail,
                        employee?.UserName ?? "Employee",
                        leaveDetail.LeaveStart ?? DateTime.MinValue,
                        leaveDetail.LeaveEnd ?? DateTime.MinValue,
                        leaveDetail.LeaveType,
                        request.ApproverEmail,
                        approverUser.UserName,
                        request.ApproverComment
                    );
                }
                catch (Exception emailEx)
                {
                    // Log email failure but don't fail the transaction
                    Console.WriteLine($"Email sending failed: {emailEx.Message}");
                }
                await transaction.CommitAsync();

                return new ApprovalResponseDto
                {
                    Success = true,
                    Message = $"Leave request rejected successfully by approver level {request.ApproverLevel}.",
                    UpdatedStatus = int.Parse(newStatus)
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException($"Error rejecting leave request: {ex.Message}", ex);
            }
        }
    }
}
