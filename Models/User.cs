using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Urban.ng.ViewModels;

namespace Urban.ng.Models
{
    //public class ApplicationUser : IdentityUser<int> USE int as Id not GUID
    public class User : IdentityUser<int>
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "User Type")]
        public string UserType { get; set; }
        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; }
        public ICollection<Property> Properties { get; set; }
        public ICollection<ProfilePhoto> ProfilePhoto { get; set; }

        [Display(Name = "Account Name")]
        public string AccountName { get; set; }
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
