using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public QuestionType Type { get; set; }
        public int OrderNumber { get; set; }

        public int SurveyId { get; set; }
        public List<QuestionOptionResponse> QuestionOptions { get; set; } = new List<QuestionOptionResponse>();
        public QuestionLineResponse QuestionLine { get; set; }
    }
}
