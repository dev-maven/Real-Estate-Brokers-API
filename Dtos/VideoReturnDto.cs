using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.ViewModels
{
    public class VideoReturnDto
    {
        public int VideoId { get; set; }
        public string VideoUrl { get; set; }
        public string VideoDescription { get; set; }
        public string VideoPublicId { get; set; }
    }
}
