using LMS.Domain.Models;
using LMS.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task<bool> SendEmailAsync(EmailRequest emailRequest)
        {
            try
            {

                Console.WriteLine("=== EMAIL DEBUG START ===");
                Console.WriteLine($"SMTP Server: {_emailSettings.SmtpServer}");
                Console.WriteLine($"SMTP Port: {_emailSettings.SmtpPort}");
                Console.WriteLine($"Username: {_emailSettings.Username}");
                Console.WriteLine($"SenderEmail: {_emailSettings.SenderEmail}");
                Console.WriteLine($"EnableSsl: {_emailSettings.EnableSsl}");
                Console.WriteLine($"To Emails: {string.Join(", ", emailRequest.ToEmails)}");
                Console.WriteLine($"Subject: {emailRequest.Subject}");

                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
                client.EnableSsl = _emailSettings.EnableSsl;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                Console.WriteLine("SMTP Client configured...");

                using var mailMessage = new MailMessage();
                // Use FromEmail from request if provided, otherwise use settings
                var fromEmail = !string.IsNullOrEmpty(emailRequest.FromEmail)
                    ? emailRequest.FromEmail
                    : _emailSettings.SenderEmail;

                var fromName = !string.IsNullOrEmpty(emailRequest.FromName)
                    ? emailRequest.FromName
                    : _emailSettings.SenderName;

                Console.WriteLine($"From Email: {fromEmail}");
                Console.WriteLine($"From Name: {fromName}");

                mailMessage.From = new MailAddress(fromEmail, fromName);

                // Add Reply-To if sending from system email but want replies to go to approver
                if (emailRequest.FromEmail != _emailSettings.SenderEmail && !string.IsNullOrEmpty(emailRequest.FromEmail))
                {
                    mailMessage.ReplyToList.Add(new MailAddress(emailRequest.FromEmail, emailRequest.FromName));
                    Console.WriteLine($"Reply-To added: {emailRequest.FromEmail}");
                }

                foreach (var email in emailRequest.ToEmails)
                {
                    mailMessage.To.Add(email);
                    Console.WriteLine($"Added To: {email}");
                }

                foreach (var email in emailRequest.CcEmails)
                {
                    mailMessage.CC.Add(email);
                    Console.WriteLine($"Added CC: {email}");
                }

                mailMessage.Subject = emailRequest.Subject;
                mailMessage.Body = emailRequest.Body;
                mailMessage.IsBodyHtml = emailRequest.IsHtml;

                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ EMAIL SENDING FAILED!");
                Console.WriteLine($"Error Type: {ex.GetType().Name}");
                Console.WriteLine($"Error Message: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Console.WriteLine("=== EMAIL DEBUG END ===");
                return false;
            }
        }

        public async Task<bool> SendLeaveApprovalEmailAsync(string employeeEmail, string employeeName,
            DateTime leaveStart, DateTime leaveEnd, string leaveType, string approverEmail, string approverName,
            string comments = null)
        {
            var emailRequest = new EmailRequest
            {
                FromEmail = approverEmail,
                FromName = approverName,
                ToEmails = new List<string> { employeeEmail },
                Subject = "Leave Request Approved",
                Body = GenerateApprovalEmailBody(employeeName, leaveStart, leaveEnd, leaveType, approverName, comments),
                IsHtml = true
            };

            return await SendEmailAsync(emailRequest);
        }

        public async Task<bool> SendLeaveRejectionEmailAsync(string employeeEmail, string employeeName,
            DateTime leaveStart, DateTime leaveEnd, string leaveType, string approverEmail, string approverName,
            string comments = null)
        {
            var emailRequest = new EmailRequest
            {
                FromEmail = approverEmail,
                FromName = approverName,
                ToEmails = new List<string> { employeeEmail },
                Subject = "Leave Request Rejected",
                Body = GenerateRejectionEmailBody(employeeName, leaveStart, leaveEnd, leaveType, approverName, comments),
                IsHtml = true
            };

            return await SendEmailAsync(emailRequest);
        }

        private string GenerateApprovalEmailBody(string employeeName, DateTime leaveStart,
            DateTime leaveEnd, string leaveType, string approverName, string comments)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #28a745;'>Leave Request Approved</h2>
                    <p>Dear {employeeName},</p>
                    <p>Your leave request has been <strong style='color: #28a745;'>APPROVED</strong>.</p>
                    
                    <h3>Leave Details:</h3>
                    <ul>
                        <li><strong>Leave Type:</strong> {leaveType}</li>
                        <li><strong>Start Date:</strong> {leaveStart:dd/MM/yyyy}</li>
                        <li><strong>End Date:</strong> {leaveEnd:dd/MM/yyyy}</li>
                        <li><strong>Approved By:</strong> {approverName}</li>
                    </ul>
                    
                    {(string.IsNullOrWhiteSpace(comments) ? "" : $"<p><strong>Comments:</strong> {comments}</p>")}
                    
                    <p>Please ensure proper handover of your responsibilities before going on leave.</p>
                    
                    <p>Best regards,<br/>
                    HR Team<br/>
                     {approverName}</p>
                </body>
                </html>";
        }

        private string GenerateRejectionEmailBody(string employeeName, DateTime leaveStart,
            DateTime leaveEnd, string leaveType, string approverName, string comments)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2 style='color: #dc3545;'>Leave Request Rejected</h2>
                    <p>Dear {employeeName},</p>
                    <p>We regret to inform you that your leave request has been <strong style='color: #dc3545;'>REJECTED</strong>.</p>
                    
                    <h3>Leave Details:</h3>
                    <ul>
                        <li><strong>Leave Type:</strong> {leaveType}</li>
                        <li><strong>Start Date:</strong> {leaveStart:dd/MM/yyyy}</li>
                        <li><strong>End Date:</strong> {leaveEnd:dd/MM/yyyy}</li>
                        <li><strong>Reviewed By:</strong> {approverName}</li>
                    </ul>
                    
                    {(string.IsNullOrWhiteSpace(comments) ? "" : $"<p><strong>Reason for Rejection:</strong> {comments}</p>")}
                    
                    <p>If you have any questions regarding this decision, please contact your supervisor or HR department.</p>
                    
                    <p>Best regards,<br/>
                     {approverName}<br/>
                    Leave Management System</p>
                </body>
                </html>";
        }
    }
}
