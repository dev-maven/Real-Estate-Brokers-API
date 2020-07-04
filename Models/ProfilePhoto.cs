using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Models
{
    public class ProfilePhoto
    {
        public int ProfilePhotoId { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string ProfilePhotoPublicId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
