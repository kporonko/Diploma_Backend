using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class Question : BaseEntity
    {
        public QuestionType Type { get; set; }
        public string QuestionLine { get; set; }
        
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public List<QuestionOption> QuestionOptions { get; set; } = new List<QuestionOption>();
    }
}
