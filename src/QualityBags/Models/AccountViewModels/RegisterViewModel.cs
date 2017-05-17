using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QualityBags.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(pattern: @"^[A-Z]+[a-zA-Z''-'\s]*$", 
            ErrorMessage = "First name must start with a capital letter and contain only alphabetic letters.")]
        [StringLength(50, ErrorMessage = "The first name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(pattern: @"^[A-Z]+[a-zA-Z''-'\s]*$",
            ErrorMessage = "Last name must start with a capital letter and contain only alphabetic letters.")]
        [StringLength(50, ErrorMessage = "The last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Display(Name = "Contact Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        [StringLength(100, 
            ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Address { get; set; }
    }
}
