using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.AccountViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "UserNameOrEmail")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }
}
