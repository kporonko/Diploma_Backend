using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class TargetingCreateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<int, string> Countries { get; set; }
        public List<int>? SurveysIds { get; set; }
    }
}
