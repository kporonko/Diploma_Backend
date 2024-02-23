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
        [EmailAddress(ErrorMessage = "You must enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must enter the password.")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must contain at least 8 symbols.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter and one digit.")]
        public string Password { get; set; }
    }
}
