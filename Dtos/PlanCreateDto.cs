using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Urban.ng.ViewModels
{
    public class PlanCreateDto
    {
        public string PlanUrl { get; set; }
        public IFormFile PlanFile { get; set; }
        public string PlanPublicId { get; set; }
    }
}
