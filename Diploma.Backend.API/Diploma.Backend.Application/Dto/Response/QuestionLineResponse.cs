using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class QuestionLineResponse
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public List<QuestionTranslationResponse> QuestionTranslations { get; set; } = new List<QuestionTranslationResponse>();
    }
}
