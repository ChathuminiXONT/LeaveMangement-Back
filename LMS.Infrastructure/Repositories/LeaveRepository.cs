using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using LMS.Domain.Models;
using LMS.Infrastructure.Data;
using LMS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LeaveRepository> _logger;
        private readonly IEmailService _emailService;

        public LeaveRepository(ApplicationDbContext context, ILogger<LeaveRepository> logger, IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        //    public async Task<ApiResponse<long>> ApplyLeaveAsync(LeaveApplicationDto leaveApplication)
        //    {
        //        try
        //        {
        //            // Check leave entitlement
        //            var hasEntitlement = await CheckLeaveEntitlementAsync(
        //                leaveApplication.EmployeeNo,
        //                leaveApplication.LeaveType,
        //                leaveApplication.LeaveDays,
        //                leaveApplication.LeaveYear);

        //            if (!hasEntitlement)
        //            {
        //                return new ApiResponse<long>
        //                {
        //                    Success = false,
        //                    Message = "Insufficient leave balance for this leave type."
        //                };
        //            }

        //            // Get username from User table using EmployeeNo
        //            var user = await _context.Users
        //                .Where(u => u.EmailAddress == leaveApplication.EmpEmailID)
        //                .Select(u => u.UserName)
        //                .FirstOrDefaultAsync();

        //            if (string.IsNullOrEmpty(user))
        //            {
        //                return new ApiResponse<long>
        //                {
        //                    Success = false,
        //                    Message = "User not found for the provided employee number."
        //                };
        //            }
        //            //himasha
        //            var leaveDetail = new LeaveDetails
        //            {
        //                BusinessUnit = leaveApplication.BusinessUnit,
        //                EmployeeNo = leaveApplication.EmployeeNo,
        //                EmpEmailID = leaveApplication.EmpEmailID,
        //                LeaveYear = leaveApplication.LeaveYear,
        //                LeaveType = leaveApplication.LeaveType,
        //                LeaveDays = leaveApplication.LeaveDays,
        //                LeaveStart = leaveApplication.LeaveStart,
        //                //StartTime = leaveApplication.StartTime,
        //                StartTime = string.IsNullOrEmpty(leaveApplication.StartTime)
        //? null
        //: TimeSpan.Parse(leaveApplication.StartTime),
        //                LeaveEnd = leaveApplication.LeaveEnd,
        //                //EndTime = leaveApplication.EndTime,
        //                EndTime = string.IsNullOrEmpty(leaveApplication.EndTime)
        //? null
        //: TimeSpan.Parse(leaveApplication.EndTime),
        //                LeaveReason = leaveApplication.LeaveReason,
        //                LeaveAppliedOn = DateTime.Now,
        //                LeaveStatus = "0", // Pending
        //                ApprovedComment = "",
        //                // CreatedBy = leaveApplication.EmployeeNo,
        //                CreatedBy = user,
        //                CreatedOn = DateTime.Now,
        //                // UpdatedBy = leaveApplication.EmployeeNo
        //                UpdatedBy = ""
        //            };

        //            _context.LeaveDetails.Add(leaveDetail);
        //            await _context.SaveChangesAsync();

        //            // Update leave entitlement (increase requested leave)
        //            await UpdateLeaveEntitlementAsync(leaveApplication.EmployeeNo, leaveApplication.LeaveType,
        //                leaveApplication.LeaveYear, leaveApplication.LeaveDays, "requested");

        //            // Send email to approver
        //            var approverEmail = await GetApproverEmailAsync(leaveApplication.EmployeeNo);
        //            if (!string.IsNullOrEmpty(approverEmail))
        //            {
        //                var leaveResponse = await GetLeaveByIdAsync(leaveDetail.RecID);
        //                if (leaveResponse.Success && leaveResponse.Data != null)
        //                {
        //                    await _emailService.SendLeaveApplicationEmailAsync(approverEmail, leaveResponse.Data);
        //                }
        //            }

        //            return new ApiResponse<long>
        //            {
        //                Success = true,
        //                Message = "Leave application submitted successfully.",
        //                Data = leaveDetail.RecID
        //            };
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error applying leave for employee {EmployeeNo}", leaveApplication.EmployeeNo);
        //            return new ApiResponse<long>
        //            {
        //                Success = false,
        //                Message = "An error occurred while applying for leave."
        //            };
        //        }
        //    }
        //new
        public async Task<ApiResponse<long>> ApplyLeaveAsync(LeaveApplicationDto leaveApplication)
        {
            try
            {
                // Always set BusinessUnit = "XONT"
                leaveApplication.BusinessUnit = "XONT";

                // Find user by EmpEmailID
                var userRecord = await _context.Users
                    .Where(u => u.EmailAddress == leaveApplication.EmpEmailID)
                    .Select(u => new { u.UserID, u.UserName })
                    .FirstOrDefaultAsync();

                if (userRecord == null)
                {
                    return new ApiResponse<long>
                    {
                        Success = false,
                        Message = "User not found for the provided email address."
                    };
                }

                // Auto-fill EmployeeNo from UserID
                leaveApplication.EmployeeNo = userRecord.UserID.ToString();

                //// Check leave entitlement
                //var hasEntitlement = await CheckLeaveEntitlementAsync(
                //    leaveApplication.EmployeeNo,
                //    leaveApplication.LeaveType,
                //    leaveApplication.LeaveDays,
                //    leaveApplication.LeaveYear);

                //if (!hasEntitlement)
                //{
                //    return new ApiResponse<long>
                //    {
                //        Success = false,
                //        Message = "Insufficient leave balance for this leave type."
                //    };
                //}

                var leaveDetail = new LeaveDetails
                {
                    BusinessUnit = leaveApplication.BusinessUnit,
                    EmployeeNo = leaveApplication.EmployeeNo,
                    EmpEmailID = leaveApplication.EmpEmailID,
                    LeaveYear = leaveApplication.LeaveYear,
                    LeaveType = leaveApplication.LeaveType,
                    LeaveDays = leaveApplication.LeaveDays,
                    LeaveStart = leaveApplication.LeaveStart,
                    StartTime = string.IsNullOrEmpty(leaveApplication.StartTime)
                        ? null
                        : TimeSpan.Parse(leaveApplication.StartTime),
                    LeaveEnd = leaveApplication.LeaveEnd,
                    EndTime = string.IsNullOrEmpty(leaveApplication.EndTime)
                        ? null
                        : TimeSpan.Parse(leaveApplication.EndTime),
                    LeaveReason = leaveApplication.LeaveReason,
                    LeaveAppliedOn = DateTime.Now,
                    LeaveStatus = "0",
                    ApprovedComment = "",
                    CreatedBy = userRecord.UserName ?? "", // auditing with UserName
                    CreatedOn = DateTime.Now,
                    UpdatedBy = ""
                };

                _context.LeaveDetails.Add(leaveDetail);
                await _context.SaveChangesAsync();

                // Update leave entitlement
                await UpdateLeaveEntitlementAsync(
                    leaveApplication.EmployeeNo,
                    leaveApplication.LeaveType,
                    leaveApplication.LeaveYear,
                    leaveApplication.LeaveDays,
                    "requested");

                // Send email to approver
                var approverEmail = await GetApproverEmailAsync(leaveApplication.EmployeeNo);
                if (!string.IsNullOrEmpty(approverEmail))
                {
                    var leaveResponse = await GetLeaveByIdAsync(leaveDetail.RecID);
                    if (leaveResponse.Success && leaveResponse.Data != null)
                    {
                        await _emailService.SendLeaveApplicationEmailAsync(approverEmail, leaveResponse.Data);
                    }
                }

                return new ApiResponse<long>
                {
                    Success = true,
                    Message = "Leave application submitted successfully.",
                    Data = leaveDetail.RecID
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying leave for employee {Email}", leaveApplication.EmpEmailID);
                return new ApiResponse<long>
                {
                    Success = false,
                    Message = "An error occurred while applying for leave."
                };
            }
        }

        public async Task<ApiResponse<List<LeaveBalanceDto>>> GetAvailableLeavesByEmailAndYearAsync(string email, int year)
{
    try
    {
        var leaveBalances = await _context.LeaveEntitlements
            .Where(le => le.EmpEmailID == email && le.LVYear == year && le.Status == "1")
            .Join(_context.LeaveTypes,
                le => le.LeaveType,
                lt => lt.LeaveType,
                (le, lt) => new LeaveBalanceDto
                {
                    LeaveType = le.LeaveType,
                    LeaveTypeName = lt.LeaveTypeName,
                    EntitledLeave = le.EntitledLeave,
                    TakenLeaves = le.TakenLeaves,
                    AvailableLeave = le.AvailableLeave,
                    RequestedLeave = le.RequestedLeave,
                    RejectedLeave = le.RejectedLeave,
                    LeaveColor = lt.LeaveColor
                })
            .ToListAsync();

        if (!leaveBalances.Any())
        {
            return new ApiResponse<List<LeaveBalanceDto>>
            {
                Success = false,
                Message = "No leave entitlements found for this email and year."
            };
}

return new ApiResponse<List<LeaveBalanceDto>>
{
    Success = true,
    Data = leaveBalances
};
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting available leaves for email {Email} year {Year}", email, year);
return new ApiResponse<List<LeaveBalanceDto>>
{
    Success = false,
    Message = "An error occurred while retrieving available leaves."
};
    }
}

        public async Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetEmployeeLeaveHistoryAsync(string employeeNo, int year)
        {
            try
            {
                var leaveHistory = await _context.LeaveDetails
                    .Where(ld => ld.EmployeeNo == employeeNo && ld.LeaveYear == year)
                    .Join(_context.LeaveTypes,
                        ld => ld.LeaveType,
                        lt => lt.LeaveType,
                        (ld, lt) => new { LeaveDetail = ld, LeaveType = lt })
                    .OrderByDescending(x => x.LeaveDetail.LeaveAppliedOn)
                    .Select(x => new LeaveApplicationResponseDto
                    {
                        RecID = x.LeaveDetail.RecID,
                        BusinessUnit = x.LeaveDetail.BusinessUnit,
                        EmployeeNo = x.LeaveDetail.EmployeeNo,
                        EmpEmailID = x.LeaveDetail.EmpEmailID,
                        LeaveYear = x.LeaveDetail.LeaveYear,
                        LeaveType = x.LeaveDetail.LeaveType,
                        LeaveTypeName = x.LeaveType.LeaveTypeName,
                        LeaveDays = x.LeaveDetail.LeaveDays,
                        LeaveStart = x.LeaveDetail.LeaveStart,
                        StartTime = x.LeaveDetail.StartTime,
                        LeaveEnd = x.LeaveDetail.LeaveEnd,
                        EndTime = x.LeaveDetail.EndTime,
                        LeaveReason = x.LeaveDetail.LeaveReason,
                        LeaveAppliedOn = x.LeaveDetail.LeaveAppliedOn,
                        LeaveStatus = x.LeaveDetail.LeaveStatus,
                        LeaveStatusText = GetLeaveStatusText(x.LeaveDetail.LeaveStatus),
                        ApprovedComment = x.LeaveDetail.ApprovedComment,
                        CanEdit = x.LeaveDetail.LeaveStatus == "0", // Can edit only if pending
                        CanDelete = x.LeaveDetail.LeaveStatus == "0" // Can delete only if pending
                    })
                    .ToListAsync();

                return new ApiResponse<List<LeaveApplicationResponseDto>>
                {
                    Success = true,
                    Data = leaveHistory
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave history for employee {EmployeeNo}", employeeNo);
                return new ApiResponse<List<LeaveApplicationResponseDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving leave history."
                };
            }
        }
        public async Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetLeavesByEmailAsync(string email)
        {
            try
            {
                var leaves = await _context.LeaveDetails
                    .Where(ld => ld.EmpEmailID == email)
                    .Join(_context.LeaveTypes,
                        ld => ld.LeaveType,
                        lt => lt.LeaveType,
                        (ld, lt) => new { LeaveDetail = ld, LeaveType = lt })
                    .Select(x => new LeaveApplicationResponseDto
                    {
                        RecID = x.LeaveDetail.RecID,
                        BusinessUnit = x.LeaveDetail.BusinessUnit,
                        EmployeeNo = x.LeaveDetail.EmployeeNo,
                        EmpEmailID = x.LeaveDetail.EmpEmailID,
                        LeaveYear = x.LeaveDetail.LeaveYear,
                        LeaveType = x.LeaveDetail.LeaveType,
                        LeaveTypeName = x.LeaveType.LeaveTypeName,
                        LeaveDays = x.LeaveDetail.LeaveDays,
                        LeaveStart = x.LeaveDetail.LeaveStart,
                        StartTime = x.LeaveDetail.StartTime,
                        LeaveEnd = x.LeaveDetail.LeaveEnd,
                        EndTime = x.LeaveDetail.EndTime,
                        LeaveReason = x.LeaveDetail.LeaveReason,
                        LeaveAppliedOn = x.LeaveDetail.LeaveAppliedOn,
                        LeaveStatus = x.LeaveDetail.LeaveStatus,
                        LeaveStatusText = GetLeaveStatusText(x.LeaveDetail.LeaveStatus),
                        ApprovedComment = x.LeaveDetail.ApprovedComment,
                        CanEdit = x.LeaveDetail.LeaveStatus == "0",
                        CanDelete = x.LeaveDetail.LeaveStatus == "0"
                    })
                    .OrderByDescending(x => x.LeaveAppliedOn)
                    .ToListAsync();

                if (!leaves.Any())
                {
                    return new ApiResponse<List<LeaveApplicationResponseDto>>
                    {
                        Success = false,
                        Message = "No leave applications found for this email."
                    };
                }

                return new ApiResponse<List<LeaveApplicationResponseDto>>
                {
                    Success = true,
                    Data = leaves
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leaves by email {Email}", email);
                return new ApiResponse<List<LeaveApplicationResponseDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving leave details."
                };
            }
        }
        public async Task<ApiResponse<LeaveApplicationResponseDto>> GetLeaveByIdAsync(long recId)
        {
            try
            {
                var leave = await _context.LeaveDetails
                    .Where(ld => ld.RecID == recId)
                    .Join(_context.LeaveTypes,
                        ld => ld.LeaveType,
                        lt => lt.LeaveType,
                        (ld, lt) => new { LeaveDetail = ld, LeaveType = lt })
                    .Select(x => new LeaveApplicationResponseDto
                    {
                        RecID = x.LeaveDetail.RecID,
                        BusinessUnit = x.LeaveDetail.BusinessUnit,
                        EmployeeNo = x.LeaveDetail.EmployeeNo,
                        EmpEmailID = x.LeaveDetail.EmpEmailID,
                        LeaveYear = x.LeaveDetail.LeaveYear,
                        LeaveType = x.LeaveDetail.LeaveType,
                        LeaveTypeName = x.LeaveType.LeaveTypeName,
                        LeaveDays = x.LeaveDetail.LeaveDays,
                        LeaveStart = x.LeaveDetail.LeaveStart,
                        StartTime = x.LeaveDetail.StartTime,
                        LeaveEnd = x.LeaveDetail.LeaveEnd,
                        EndTime = x.LeaveDetail.EndTime,
                        LeaveReason = x.LeaveDetail.LeaveReason,
                        LeaveAppliedOn = x.LeaveDetail.LeaveAppliedOn,
                        LeaveStatus = x.LeaveDetail.LeaveStatus,
                        LeaveStatusText = GetLeaveStatusText(x.LeaveDetail.LeaveStatus),
                        ApprovedComment = x.LeaveDetail.ApprovedComment,
                        CanEdit = x.LeaveDetail.LeaveStatus == "0",
                        CanDelete = x.LeaveDetail.LeaveStatus == "0"
                    })
                    .FirstOrDefaultAsync();

                if (leave == null)
                {
                    return new ApiResponse<LeaveApplicationResponseDto>
                    {
                        Success = false,
                        Message = "Leave application not found."
                    };
                }

                return new ApiResponse<LeaveApplicationResponseDto>
                {
                    Success = true,
                    Data = leave
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave by ID {RecId}", recId);
                return new ApiResponse<LeaveApplicationResponseDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving leave details."
                };
            }
        }

        public async Task<ApiResponse<bool>> UpdateLeaveAsync(long recId, LeaveApplicationDto leaveApplication)
        {
            try
            {
                var existingLeave = await _context.LeaveDetails.FindAsync(recId);
                if (existingLeave == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Leave application not found."
                    };
                }

                if (existingLeave.LeaveStatus != "0") // Only pending leaves can be updated
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Only pending leave applications can be updated."
                    };
                }

                // Check leave entitlement for the updated days
                var daysDifference = leaveApplication.LeaveDays - existingLeave.LeaveDays;
                if (daysDifference > 0)
                {
                    var hasEntitlement = await CheckLeaveEntitlementAsync(
                        leaveApplication.EmployeeNo,
                        leaveApplication.LeaveType,
                        daysDifference,
                        leaveApplication.LeaveYear);

                    if (!hasEntitlement)
                    {
                        return new ApiResponse<bool>
                        {
                            Success = false,
                            Message = "Insufficient leave balance for the updated leave days."
                        };
                    }
                }
                // Get username from User table using EmployeeNo
                string user;
                try
                {
                    user = await _context.Users  // Changed from Users to User (singular)
                        .Where(u => u.EmailAddress == leaveApplication.EmpEmailID)  // Changed to LoginID and EmployeeNo
                        .Select(u => u.UserName)
                        .FirstOrDefaultAsync();
                }
                catch (Exception userQueryEx)
                {
                    _logger.LogError(userQueryEx, "Error querying user for employee {EmployeeNo}", leaveApplication.EmployeeNo);
                    return new ApiResponse<bool>  // Changed return type to bool
                    {
                        Success = false,
                        Message = $"Error retrieving user information: {userQueryEx.Message}"
                    };
                }

                if (string.IsNullOrEmpty(user))
                {
                    _logger.LogWarning("User not found for employee number {EmployeeNo}", leaveApplication.EmployeeNo);
                    return new ApiResponse<bool>  // Changed return type to bool
                    {
                        Success = false,
                        Message = "User not found for the provided employee number."
                    };
                }
                // Update leave entitlement
                await UpdateLeaveEntitlementAsync(existingLeave.EmployeeNo, existingLeave.LeaveType,
                    existingLeave.LeaveYear, -existingLeave.LeaveDays, "requested");

                // Update leave details
                existingLeave.LeaveType = leaveApplication.LeaveType;
                existingLeave.LeaveDays = leaveApplication.LeaveDays;
                existingLeave.LeaveStart = leaveApplication.LeaveStart;
                //existingLeave.StartTime = leaveApplication.StartTime;
                existingLeave.StartTime = string.IsNullOrEmpty(leaveApplication.StartTime)
    ? null
    : TimeSpan.Parse(leaveApplication.StartTime);
                existingLeave.LeaveEnd = leaveApplication.LeaveEnd;
                //existingLeave.EndTime = leaveApplication.EndTime;
                existingLeave.EndTime = string.IsNullOrEmpty(leaveApplication.EndTime)
    ? null
    : TimeSpan.Parse(leaveApplication.EndTime);
                existingLeave.LeaveReason = leaveApplication.LeaveReason;
                //existingLeave.UpdatedBy = leaveApplication.EmployeeNo;
                existingLeave.UpdatedBy =user;
                existingLeave.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();

                // Update leave entitlement with new values
                await UpdateLeaveEntitlementAsync(existingLeave.EmployeeNo, existingLeave.LeaveType,
                    existingLeave.LeaveYear, existingLeave.LeaveDays, "requested");

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Leave application updated successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating leave {RecId}", recId);
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while updating the leave application."
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteLeaveAsync(long recId)
        {
            try
            {
                var existingLeave = await _context.LeaveDetails.FindAsync(recId);
                if (existingLeave == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Leave application not found."
                    };
                }

                if (existingLeave.LeaveStatus != "0") // Only pending leaves can be deleted
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Only pending leave applications can be deleted."
                    };
                }

                // Update leave entitlement (decrease requested leave)
                await UpdateLeaveEntitlementAsync(existingLeave.EmployeeNo, existingLeave.LeaveType,
                    existingLeave.LeaveYear, -existingLeave.LeaveDays, "requested");

                _context.LeaveDetails.Remove(existingLeave);
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Leave application deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting leave {RecId}", recId);
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while deleting the leave application."
                };
            }
        }

        public async Task<ApiResponse<bool>> ApproveRejectLeaveAsync(LeaveApprovalDto approval)
        {
            try
            {
                var existingLeave = await _context.LeaveDetails.FindAsync(approval.RecID);
                if (existingLeave == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Leave application not found."
                    };
                }

                if (existingLeave.LeaveStatus != "0")
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Leave application has already been processed."
                    };
                }

                existingLeave.LeaveStatus = approval.LeaveStatus;
                existingLeave.ApprovedComment = approval.ApprovedComment;
                existingLeave.UpdatedBy = approval.ApproverEmail;
                existingLeave.UpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();

                // Update leave entitlement based on approval/rejection
                if (approval.LeaveStatus == "1") // Approved
                {
                    // Move from requested to taken
                    await UpdateLeaveEntitlementAsync(existingLeave.EmployeeNo, existingLeave.LeaveType,
                        existingLeave.LeaveYear, -existingLeave.LeaveDays, "requested");
                    await UpdateLeaveEntitlementAsync(existingLeave.EmployeeNo, existingLeave.LeaveType,
                        existingLeave.LeaveYear, existingLeave.LeaveDays, "taken");
                }
                else if (approval.LeaveStatus == "3") // Rejected
                {
                    // Move from requested to rejected
                    await UpdateLeaveEntitlementAsync(existingLeave.EmployeeNo, existingLeave.LeaveType,
                        existingLeave.LeaveYear, -existingLeave.LeaveDays, "requested");
                    await UpdateLeaveEntitlementAsync(existingLeave.EmployeeNo, existingLeave.LeaveType,
                        existingLeave.LeaveYear, existingLeave.LeaveDays, "rejected");
                }

                // Send email to employee
                var leaveResponse = await GetLeaveByIdAsync(approval.RecID);
                if (leaveResponse.Success && leaveResponse.Data != null)
                {
                    await _emailService.SendLeaveStatusEmailAsync(
                        existingLeave.EmpEmailID,
                        leaveResponse.Data,
                        approval.LeaveStatus == "1");
                }

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = $"Leave application {(approval.LeaveStatus == "1" ? "approved" : "rejected")} successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving/rejecting leave {RecId}", approval.RecID);
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while processing the leave application."
                };
            }
        }

        public async Task<ApiResponse<List<LeaveBalanceDto>>> GetEmployeeLeaveBalanceAsync(string employeeNo, int year)
        {
            try
            {
                var leaveBalances = await _context.LeaveEntitlements
                    .Where(le => le.EmpNo == employeeNo && le.LVYear == year && le.Status == "1")
                    .Join(_context.LeaveTypes,
                        le => le.LeaveType,
                        lt => lt.LeaveType,
                        (le, lt) => new LeaveBalanceDto
                        {
                            LeaveType = le.LeaveType,
                            LeaveTypeName = lt.LeaveTypeName,
                            EntitledLeave = le.EntitledLeave,
                            TakenLeaves = le.TakenLeaves,
                            AvailableLeave = le.AvailableLeave,
                            RequestedLeave = le.RequestedLeave,
                            RejectedLeave = le.RejectedLeave,
                            LeaveColor = lt.LeaveColor
                        })
                    .ToListAsync();

                return new ApiResponse<List<LeaveBalanceDto>>
                {
                    Success = true,
                    Data = leaveBalances
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting leave balance for employee {EmployeeNo}", employeeNo);
                return new ApiResponse<List<LeaveBalanceDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving leave balance."
                };
            }
        }

        public async Task<ApiResponse<List<LeaveApplicationResponseDto>>> GetPendingApprovalsAsync(string approverEmail)
        {
            try
            {
                var pendingApprovals = await _context.LeaveDetails
                    .Where(ld => ld.LeaveStatus == "0") // Pending status
                    .Join(_context.Users,
                        ld => ld.EmployeeNo,
                        u => u.LoginID,
                        (ld, u) => new { LeaveDetail = ld, User = u })
                    .Join(_context.Departments,
                        x => x.User.DepartmentID,
                        d => d.Department,
                        (x, d) => new { x.LeaveDetail, x.User, Department = d })
                    .Where(x => x.Department.ApproverEmp1 == approverEmail || x.Department.ApproverEmp2 == approverEmail)
                    .Join(_context.LeaveTypes,
                        x => x.LeaveDetail.LeaveType,
                        lt => lt.LeaveType,
                        (x, lt) => new LeaveApplicationResponseDto
                        {
                            RecID = x.LeaveDetail.RecID,
                            BusinessUnit = x.LeaveDetail.BusinessUnit,
                            EmployeeNo = x.LeaveDetail.EmployeeNo,
                            EmpEmailID = x.LeaveDetail.EmpEmailID,
                            LeaveYear = x.LeaveDetail.LeaveYear,
                            LeaveType = x.LeaveDetail.LeaveType,
                            LeaveTypeName = lt.LeaveTypeName,
                            LeaveDays = x.LeaveDetail.LeaveDays,
                            LeaveStart = x.LeaveDetail.LeaveStart,
                            StartTime = x.LeaveDetail.StartTime,
                            LeaveEnd = x.LeaveDetail.LeaveEnd,
                            EndTime = x.LeaveDetail.EndTime,
                            LeaveReason = x.LeaveDetail.LeaveReason,
                            LeaveAppliedOn = x.LeaveDetail.LeaveAppliedOn,
                            LeaveStatus = x.LeaveDetail.LeaveStatus,
                            LeaveStatusText = "Pending",
                            ApprovedComment = x.LeaveDetail.ApprovedComment,
                            ApproverName = x.User.UserName ?? "",
                            CanEdit = false,
                            CanDelete = false
                        })
                    .OrderBy(x => x.LeaveAppliedOn)
                    .ToListAsync();

                return new ApiResponse<List<LeaveApplicationResponseDto>>
                {
                    Success = true,
                    Data = pendingApprovals
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending approvals for {ApproverEmail}", approverEmail);
                return new ApiResponse<List<LeaveApplicationResponseDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving pending approvals."
                };
            }
        }

        public async Task<bool> CheckLeaveEntitlementAsync(string employeeNo, string leaveType, int leaveDays, int year)
        {
            try
            {
                var entitlement = await _context.LeaveEntitlements
                    .FirstOrDefaultAsync(le => le.EmpNo == employeeNo &&
                                               le.LeaveType == leaveType &&
                                               le.LVYear == year &&
                                               le.Status == "1");

                if (entitlement == null)
                    return false;

                return entitlement.AvailableLeave >= leaveDays;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking leave entitlement for {EmployeeNo}", employeeNo);
                return false;
            }
        }

        public async Task<string?> GetApproverEmailAsync(string employeeNo)
        {
            try
            {
                var approver = await _context.Users
                    .Where(u => u.LoginID == employeeNo)
                    .Join(_context.Departments,
                        u => u.DepartmentID,
                        d => d.Department,
                        (u, d) => d.ApproverEmp1)
                    .FirstOrDefaultAsync();

                return approver;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting approver email for {EmployeeNo}", employeeNo);
                return null;
            }
        }

        private async Task UpdateLeaveEntitlementAsync(string employeeNo, string leaveType, int year, int days, string type)
        {
            try
            {
                var entitlement = await _context.LeaveEntitlements
                    .FirstOrDefaultAsync(le => le.EmpNo == employeeNo &&
                                               le.LeaveType == leaveType &&
                                               le.LVYear == year);

                if (entitlement != null)
                {
                    switch (type.ToLower())
                    {
                        case "requested":
                            entitlement.RequestedLeave += days;
                            //entitlement.AvailableLeave -= days;
                            break;
                        case "taken":
                            entitlement.TakenLeaves += days;
                            break;
                        case "rejected":
                            entitlement.RejectedLeave += days;
                            entitlement.AvailableLeave += days;
                            break;
                    }
                    // Get username from User table using EmployeeNo (convert string to int)
                    string user = null;
                    if (int.TryParse(employeeNo, out int empId))
                    {
                        user = await _context.Users
                            .Where(u => u.UserID == empId)
                            .Select(u => u.UserName)
                            .FirstOrDefaultAsync();
                    }

                    entitlement.UpdatedBy = user;
                    entitlement.UpdatedOn = DateTime.Now;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating leave entitlement for {EmployeeNo}", employeeNo);
            }
        }

        private static string GetLeaveStatusText(string status)
        {
            return status switch
            {
                "0" => "Pending",
                "1" => "Approved",
                "2" => "Approved",
                "3" => "Rejected",
                "4" => "Rejected",
                _ => "Unknown"
            };
        }
    }
}
