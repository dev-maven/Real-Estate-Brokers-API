using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }

        public string PhotoUrl { get; set; }

        public string PhotoPublicId { get; set; }
        public Property Property { get; set; }
        public int PropertyId { get; set; }
        public bool IsMain { get; set; }
    }
}
