using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class QuestionTranslation : BaseEntity
    {
        public string QuestionTranslationLine { get; set; }
        public string Language { get; set; }

        public int QuestionLineId { get; set; }
        public QuestionLine QuestionLine { get; set; }
    }
}
