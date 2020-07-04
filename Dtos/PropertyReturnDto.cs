using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Urban.ng.Models;
using Urban.ng.ViewModels;

namespace Urban.ng.Dtos
{
    public class PropertyReturnDto
    {
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Sale { get; set; }
        public int Total { get; set; }
        public string Location { get; set; }
        public IList<FeatureReturnDto> Features { get; set; }
        public int OtherCharges { get; set; }
        public ICollection<PhotoReturnDto> Photos { get; set; }
        public ICollection<VideoReturnDto> Videos { get; set; }
        public ICollection<PlanReturnDto> Plans { get; set; }
        public DateTime DateAdded { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
