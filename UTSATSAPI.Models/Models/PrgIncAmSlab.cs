using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgIncAmSlab
    {
        public int Id { get; set; }
        public string? Slab { get; set; }
        public decimal? MinLimitPercentage { get; set; }
        public decimal? MaxLimitPercentage { get; set; }
        public bool? IsActive { get; set; }
    }
}
