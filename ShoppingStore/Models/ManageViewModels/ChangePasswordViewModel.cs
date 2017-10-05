using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage ="RequiredError")]
        [DataType(DataType.Password)]
        [Display(Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [StringLength(100, ErrorMessage = "StringLengthError", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessage = "ConfirmNewPasswordError")]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}
