using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class Template : BaseEntity
    {
        public string TemplateCode { get; set; }
        public string DefaultParams { get; set; }

        public List<UnitAppearance> UnitAppearances { get; set; } = new List<UnitAppearance>();
    }
}
