using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required(ErrorMessage ="RequiredError")]
        [EmailAddress(ErrorMessage ="EmailAddressError")]
        public string Email { get; set; }
    }
}
