using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class QuestionOption : BaseEntity
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public List<OptionTranslation> OptionTranslations { get; set; } = new List<OptionTranslation>();
    }
}
