using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Service.DTOs.Section
{
    public class UpdateSectionDto
    {
        public string Title { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
