using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Core.Helper
{
    public class MailSetting
    {

        public string? FromEmail { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? SmtpHost { get; set; }
        public string? SmtpPort { get; set; }

    }
}
