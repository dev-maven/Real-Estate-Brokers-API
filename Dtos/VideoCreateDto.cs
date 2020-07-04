using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.ViewModels
{
    public class VideoCreateDto
    {
        public string VideoUrl { get; set; }
        public IFormFile VideoFile { get; set; }
        public string VideoPublicId { get; set; }
    }
}
