using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class SurveyResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateBy { get; set; }
        public List<QuestionResponse> Questions { get; set; }
        public TargetingResponse Targeting { get; set; }
    }
}
