using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class TemplateCreateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TemplateCode { get; set; }
        public Dictionary<string, string> DefaultParams { get; set; }
    }
}
