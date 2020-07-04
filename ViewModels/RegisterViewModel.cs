using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
        public string LastName { get; set; }       

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailInUse", controller:"Auth")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match!")]
        public string ConfirmPassword { get; set; }
        public string UserType { get; set; }

        public DateTime DateRegistered { get; set; }

        public RegisterViewModel()
        {
            DateRegistered = DateTime.Now;
            
        }

    }

  

}
