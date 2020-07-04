using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urban.ng.Models;

namespace Urban.ng.Dtos
{
    public class PropertyForListDto
    {
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Sale { get; set; }
        public int Total { get; set; }
        public string Location { get; set; }
        public IList<Feature> Features { get; set; }
        public int OtherCharges { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
    }
}
