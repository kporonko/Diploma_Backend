using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class QuestionLine : BaseEntity
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public List<QuestionTranslation> QuestionTranslations { get; set; } = new List<QuestionTranslation>();
    }
}
