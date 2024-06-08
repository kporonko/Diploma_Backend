using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class OptionTranslationResponse
    {
        public int Id { get; set; }
        public string OptionLine { get; set; }
        public string Language { get; set; }

        public int QuestionOptionId { get; set; }
    }
}
