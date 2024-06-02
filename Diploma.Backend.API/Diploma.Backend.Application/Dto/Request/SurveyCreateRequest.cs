using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class SurveyCreateRequest
    {
        public string Name { get; set; }
        public DateTime DateBy { get; set; }
        public List<SurveyCreateRequestQuestion> Questions { get; set; }
        public int TargetingId { get; set; }
    }

    public class SurveyCreateRequestQuestion
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int OrderNumber { get; set; }
        public SurveyCreateRequestQuestionLine QuestionLine { get; set; }
        public List<SurveyCreateRequestQuestionOption> QuestionOptions { get; set; }
    }

    public class SurveyCreateRequestQuestionLine
    {
        public int Id { get; set; }
        public List<SurveyCreateRequestTranslation> Translations { get; set; }
    }

    public class SurveyCreateRequestQuestionOption
    {
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public List<SurveyCreateRequestTranslation> Translations { get; set; }
    }

    public class SurveyCreateRequestTranslation
    {
        public string LanguageCode { get; set; }
        public string TranslationText { get; set; }
    }
}
