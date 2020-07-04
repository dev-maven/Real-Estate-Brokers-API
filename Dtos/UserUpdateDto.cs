using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Dtos
{
    public class UserUpdateDto
    {
        [Display(Name = "Profile Picture")]
        public string PhotoUrl { get; set; }
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
    }
}
