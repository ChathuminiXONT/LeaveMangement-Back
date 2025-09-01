using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models
{
    public class EmailRequest
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public List<string> ToEmails { get; set; } = new List<string>();
        public List<string> CcEmails { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; } = true;
    }
}
