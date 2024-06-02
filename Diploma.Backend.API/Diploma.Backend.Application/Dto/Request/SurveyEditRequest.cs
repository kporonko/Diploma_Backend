﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class SurveyEditRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateBy { get; set; }
        public List<SurveyCreateRequestQuestion> Questions { get; set; }
        public int TargetingId { get; set; }
    }
}
