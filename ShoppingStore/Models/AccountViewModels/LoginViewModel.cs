using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="RequiredError")]
        [EmailAddress(ErrorMessage = "EmailAddressError")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="RequiredError")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
