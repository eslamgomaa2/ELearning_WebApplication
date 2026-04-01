using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Student
{
    public class EmailSecurityDto
    {
        public string? Email { get; set; }
        public bool IsVerified {get; set; }
    }
}
