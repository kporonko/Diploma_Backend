﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class UnitAppearanceCreateRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int TemplateId { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string> Params { get; set; }
    }
}
