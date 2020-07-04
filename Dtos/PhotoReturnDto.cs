using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.ViewModels
{
    public class PhotoReturnDto
    {
        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsMain { get; set; }
        public string PhotoPublicId { get; set; }
    }
}
