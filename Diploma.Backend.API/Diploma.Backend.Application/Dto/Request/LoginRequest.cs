using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "You must enter your email address.")]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must contain at least 8 symbols.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
