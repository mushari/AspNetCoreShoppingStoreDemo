using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required(ErrorMessage ="RequiredError")]
        [StringLength(7, ErrorMessage = "StringLength", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "AuthenticatorCode")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "RememberThisMachine")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
}
