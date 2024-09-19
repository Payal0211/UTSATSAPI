using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_HiringTypeDetails_ClientPortal_Result
    {
        public long Id { get; set; }
        public string? Type { get; set; }
        public int? HrtypeId { get; set; }
        public decimal? PricingPercent { get; set; }
        public string? DisplayPricingPercent { get; set; }
        public bool? ShowPartTime { get; set; }
        public bool? IsTransparentModel { get; set; }
    }
}
