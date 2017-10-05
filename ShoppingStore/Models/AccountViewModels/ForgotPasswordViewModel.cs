using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "RequiredError")]
        [EmailAddress(ErrorMessage = "EmailAddressError")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
