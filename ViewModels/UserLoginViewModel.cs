using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.ViewModels
{
    public class UserLoginViewModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }

        public DateTime DateRegistered { get; set; }
    }
}
