using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class TemplateEditRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> DefaultParams { get; set; }
    }
}
