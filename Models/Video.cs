using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Models
{
    public class Video
    {
        public int VideoId { get; set; }
        public string VideoUrl { get; set; }
        public string VideoPublicId { get; set; }
        public Property Property { get; set; }
        public int PropertyId { get; set; }
    }
}
