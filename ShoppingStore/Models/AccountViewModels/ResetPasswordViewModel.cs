using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="RequiredError")]
        [EmailAddress(ErrorMessage = "EmailAddressError")]
        [Display(Name ="Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="RequiredError")]
        [StringLength(100, ErrorMessage = "StringLengthError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "ConfirmPasswordError")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
