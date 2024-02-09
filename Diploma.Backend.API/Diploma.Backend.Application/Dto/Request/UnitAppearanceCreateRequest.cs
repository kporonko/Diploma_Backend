using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class UnitAppearanceCreateRequest
    {
        public int? Id { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int TemplateId { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Type { get; set; }

        [Required]
        public Dictionary<string, string> Params { get; set; }
    }
}
