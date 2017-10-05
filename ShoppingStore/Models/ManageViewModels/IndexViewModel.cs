using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [EmailAddress(ErrorMessage = "EmailAddressError")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
