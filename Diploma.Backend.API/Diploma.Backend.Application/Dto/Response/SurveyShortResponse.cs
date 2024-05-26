using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class SurveyShortResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateBy { get; set; }
        public int NumberOfQuestions { get; set; }
    }
}
