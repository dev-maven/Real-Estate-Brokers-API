using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Models
{
    public class Feature
    {
        public int FeatureId { get; set; }
        public string Name { get; set; }
        public Property Property { get; set; }
        public int PropertyId { get; set; }
    }
}
