using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.ViewModels
{
    public class PhotoCreateDto
    {
        public string PhotoUrl { get; set; }
        public IFormFile PhotoFile { get; set; }
        public string PhotoPublicId { get; set; }
    }
}
