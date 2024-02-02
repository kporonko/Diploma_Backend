﻿using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class UnitSettings : BaseEntity
    {
        public int OneSurveyTakePerDevice { get; set; }
        public int MaximumSurveysPerDevice { get; set; }
        public bool HideAfterNoSurveys { get; set; }
        public bool MessageAfterNoSurveys { get; set; }

        public List<SurveyUnit> SurveyUnits { get; set; } = new List<SurveyUnit>();

    }
}
