using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.DTOs
{
    public class ApprovalResponseDto
    {

        public bool Success { get; set; }
        public string Message { get; set; }
        public int UpdatedStatus { get; set; }
    }
}
