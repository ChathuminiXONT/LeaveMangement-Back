//using LMS.Domain.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LMS.Application.Services
//{
//    public interface IEmailService
//    {
//        Task<bool> SendEmailAsync(EmailRequest emailRequest);
//        Task<bool> SendLeaveApprovalEmailAsync(string employeeEmail, string employeeName,
//            DateTime leaveStart, DateTime leaveEnd, string leaveType, string approverEmail, string approverName,
//            string comments = null);
//        Task<bool> SendLeaveRejectionEmailAsync(string employeeEmail, string employeeName,
//            DateTime leaveStart, DateTime leaveEnd, string leaveType, string approverEmail, string approverName,
//            string comments = null);
//    }
//}
