using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Profiles.Admin
{
    public class GradeRangeDto
    {
        public int? Id { get; set; }   
        public string Letter { get; set; } = string.Empty;
        public int MinScore { get; set; }
        public int MaxScore { get; set; }
    }
}
