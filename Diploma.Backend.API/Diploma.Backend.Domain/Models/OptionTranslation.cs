using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class OptionTranslation : BaseEntity
    {
        public string OptionLine { get; set; }
        public string Language { get; set; }

        public int QuestionOptionId { get; set; }
        public QuestionOption QuestionOption { get; set; }
    }
}
