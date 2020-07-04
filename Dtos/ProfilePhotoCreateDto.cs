using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Dtos
{
    public class ProfilePhotoCreateDto
    {
        public string ProfilePhotoUrl { get; set; }
        public IFormFile ProfilePhotoFile { get; set; }
        public string ProfilePhotoPublicId { get; set; }
    }
}
