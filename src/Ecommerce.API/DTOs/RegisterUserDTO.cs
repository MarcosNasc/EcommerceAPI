using System.ComponentModel.DataAnnotations;
using Ecommerce.API.Resources;

namespace Ecommerce.API.DTOs
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessageResourceType = typeof(Validations),ErrorMessageResourceName = "FieldRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidFormat")]
        public string? Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Validations),ErrorMessageResourceName = "FieldRequired")]
        [StringLength(100, ErrorMessageResourceType = typeof(Validations),ErrorMessageResourceName = "StringLength",MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessageResourceType = typeof(Validations), ErrorMessageResourceName = "InvalidPassword")]
        public string? ConfirmPassword { get; set; }
    }
}
