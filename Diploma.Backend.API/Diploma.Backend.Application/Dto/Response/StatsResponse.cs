using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class StatsResponse
    {
        public StatsForQuestion StatsForQuestion { get; set; }
        public StatsForOption StatsForOption { get; set; }
        public StatsForGender StatsForGender { get; set; }
        public StatsForGeo StatsForGeo { get; set; }
        public StatsForLang StatsForLang { get; set; }

    }

    public class StatsForQuestion
    {
        public int QuestionId { get; set; }
        public int Views { get; set; }
        public int Answered { get; set; }
    }

    public class StatsForOption
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public int Answered { get; set; }
    }
    
    public class StatsForGender
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string Gender { get; set; }
        public int Answered { get; set; }
    }

    public class StatsForGeo
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string Geo { get; set; }
        public int Answered { get; set; }
    }

    public class StatsForLang
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string Lang { get; set; }
        public int Answered { get; set; }
    }
}
