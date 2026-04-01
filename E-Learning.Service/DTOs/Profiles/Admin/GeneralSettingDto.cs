using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class GeneralSettingDto
    {
        public string DefaultLanguage { get; set; } = "en";
        public string DefaultTimeZone { get; set; } = "UTC-5";
    }
}
