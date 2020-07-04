using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Services
{
    public interface IEmailSend
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
  
}
