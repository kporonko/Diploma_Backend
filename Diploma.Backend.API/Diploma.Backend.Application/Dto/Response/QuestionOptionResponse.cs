using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class QuestionOptionResponse
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int OrderNumber { get; set; }

        public List<OptionTranslationResponse> OptionTranslations { get; set; } = new List<OptionTranslationResponse>();
    }
}
