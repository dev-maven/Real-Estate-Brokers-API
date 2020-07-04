using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Dtos
{
    public class PropertyEditDto
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool isNaira { get; set; }
        public bool isVr { get; set; }
        [Required]
        public int Sale { get; set; }
        public int Legal { get; set; }
        public int Agency { get; set; }
        public int Caution { get; set; }
        public int Total { get; set; }
        [Required]
        public string PaymentOption { get; set; }
        [Required]
        public string PropertyType { get; set; }
        public string PropertyStatus { get; set; }
        [Required]
        public string Location { get; set; }
        public string Street { get; set; }
        public string LocalGovernment { get; set; }
        public string Bedrooms { get; set; }
        public string Bathrooms { get; set; }
        public string CarPark { get; set; }
        public string Area { get; set; }
        public int otherCharges { get; set; }
        public bool Featured { get; set; }
    }
}
