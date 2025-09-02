using LMS.Domain.DTOs;
using LMS.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace LMS.Infrastructure.Services
{
    public class EmailService2 : IEmailService2
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService2> _logger;

        public EmailService2(IConfiguration configuration, ILogger<EmailService2> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendLeaveApplicationEmailAsync(string approverEmail, LeaveApplicationResponseDto leaveDetails)
        {
            try
            {
                var subject = $"New Leave Application - {leaveDetails.EmpEmailID}";
                var body = $@"
                    <html>
                    <body>
                        <h3>New Leave Application Received</h3>
                        <p><strong>Employee:</strong> {leaveDetails.EmpEmailID}</p>
                        <p><strong>Leave Type:</strong> {leaveDetails.LeaveTypeName}</p>
                        <p><strong>From Date:</strong> {leaveDetails.LeaveStart:dd/MM/yyyy}</p>
                        <p><strong>To Date:</strong> {leaveDetails.LeaveEnd:dd/MM/yyyy}</p>
                        <p><strong>Days:</strong> {leaveDetails.LeaveDays}</p>
                        <p><strong>Reason:</strong> {leaveDetails.LeaveReason}</p>
                        <p><strong>Applied On:</strong> {leaveDetails.LeaveAppliedOn:dd/MM/yyyy HH:mm}</p>
                        <br/>
                        <p>Please log in to the system to approve or reject this application.</p>
                    </body>
                    </html>";

                return await SendEmailAsync(approverEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending leave application email to {ApproverEmail}", approverEmail);
                return false;
            }
        }

        public async Task<bool> SendLeaveStatusEmailAsync(string employeeEmail, LeaveApplicationResponseDto leaveDetails, bool isApproved)
        {
            try
            {
                var status = isApproved ? "Approved" : "Rejected";
                var subject = $"Leave Application {status}";
                var body = $@"
                    <html>
                    <body>
                        <h3>Leave Application {status}</h3>
                        <p>Dear Employee,</p>
                        <p>Your leave application has been <strong>{status.ToLower()}</strong>.</p>
                        <br/>
                        <p><strong>Leave Details:</strong></p>
                        <p><strong>Leave Type:</strong> {leaveDetails.LeaveTypeName}</p>
                        <p><strong>From Date:</strong> {leaveDetails.LeaveStart:dd/MM/yyyy}</p>
                        <p><strong>To Date:</strong> {leaveDetails.LeaveEnd:dd/MM/yyyy}</p>
                        <p><strong>Days:</strong> {leaveDetails.LeaveDays}</p>
                        <p><strong>Reason:</strong> {leaveDetails.LeaveReason}</p>
                        {(string.IsNullOrEmpty(leaveDetails.ApprovedComment) ? "" : $"<p><strong>Comments:</strong> {leaveDetails.ApprovedComment}</p>")}
                        <br/>
                        <p>Thank you.</p>
                    </body>
                    </html>";

                return await SendEmailAsync(employeeEmail, subject, body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending leave status email to {EmployeeEmail}", employeeEmail);
                return false;
            }
        }

        private async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var smtpHost = smtpSettings["Host"];
                var smtpPort = int.Parse(smtpSettings["Port"] ?? "587");
                var smtpUsername = smtpSettings["Username"];
                var smtpPassword = smtpSettings["Password"];
                var smtpEnableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true");
                var fromEmail = smtpSettings["FromEmail"];

                using var client = new SmtpClient(smtpHost, smtpPort)
                {
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = smtpEnableSsl
                };

                var mailMessage = new MailMessage(fromEmail!, toEmail, subject, body)
                {
                    IsBodyHtml = true
                };

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {ToEmail}", toEmail);
                return false;
            }
        }
    }
}