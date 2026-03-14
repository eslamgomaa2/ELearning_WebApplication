using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Reviews_Certificates.Certificates
{
    public class UpdateCertificateDto
    {
        public string CertificateCode { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
        public string? FileUrl { get; set; }
    }
}
