using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "You must enter your email address.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password must contain at least 8 symbols.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must enter your first name.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must contain at least 2 symbols.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must enter your last name.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must contain at least 2 symbols.")]
        public string LastName { get; set; }
    }
}
