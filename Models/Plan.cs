using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.Models
{
    public class Plan
    {
        public int PlanId { get; set; }

        public string PlanUrl { get; set; }
        public string PlanPublicId { get; set; }
        public Property Property { get; set; }
        public int PropertyId { get; set; }
    }
}
