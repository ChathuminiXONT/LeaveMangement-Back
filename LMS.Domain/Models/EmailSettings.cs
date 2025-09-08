using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models
{
    //public class EmailSettings
    //{
    //    public string SmtpServer { get; set; }
    //    public int SmtpPort { get; set; }
    //    public string SenderEmail { get; set; }
    //    public string SenderName { get; set; }
    //    public string Username { get; set; }
    //    public string Password { get; set; }
    //    public bool EnableSsl { get; set; }
    //}

    public class EmailSettings
    {
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string ClientSecret { get; set; }
    }
}
