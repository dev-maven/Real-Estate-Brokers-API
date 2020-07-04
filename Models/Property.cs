using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Models
{
    public class Property
    {
        public int PropertyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isNaira { get; set; }
        public bool isVr { get; set; }
        public int Sale { get; set; }
        public int Legal { get; set; }
        public int Agency { get; set; }
        public int Caution { get; set; }
        public int Total { get; set; }
        public string PaymentOption { get; set; }
        public string PropertyType { get; set; }
        public string PropertyStatus { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string LocalGovernment { get; set; }
        public string Bedrooms { get; set; }
        public string Bathrooms { get; set; }
        public string CarPark { get; set; }
        public string Area { get; set; }
        public IList<Feature> Features { get; set; }
        public int OtherCharges { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Video> Videos { get; set; }
        public ICollection<Plan> Plans { get; set; }
        public bool Featured { get; set; }

        public DateTime DateAdded { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
